using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.JSInterop;

namespace event_studio;

public sealed class EventStudioState
{
    private const string EventKey = "gatherwell.event.v1";
    private const string AttendeesKey = "gatherwell.attendees.v1";
    private const string SessionKey = "gatherwell.session.v1";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private bool _loaded;

    public EventDetails Event { get; private set; } = new();
    public List<Attendee> Attendees { get; private set; } = [];
    public UserSession? Session { get; private set; }
    public int SeatsRemaining => Math.Max(0, Event.Capacity - Attendees.Count);
    public int CheckedInCount => Attendees.Count(attendee => attendee.CheckedIn);

    public async Task LoadAsync(IJSRuntime js)
    {
        if (_loaded) return;
        _loaded = true;
        Event = await ReadAsync(js, "local", EventKey, new EventDetails()) ?? new EventDetails();
        Attendees = await ReadAsync(js, "local", AttendeesKey, new List<Attendee>()) ?? [];
        Session = await ReadAsync<UserSession>(js, "session", SessionKey, null);
    }

    public Task SaveEventAsync(IJSRuntime js) => WriteAsync(js, "local", EventKey, Event);
    public Task SaveAttendeesAsync(IJSRuntime js) => WriteAsync(js, "local", AttendeesKey, Attendees);

    public async Task SignInAsync(IJSRuntime js, string name)
    {
        Session = new UserSession(name);
        await WriteAsync(js, "session", SessionKey, Session);
    }

    public async Task SignOutAsync(IJSRuntime js)
    {
        Session = null;
        await js.InvokeVoidAsync("gatherwellStorage.remove", "session", SessionKey);
    }

    private static async Task<T?> ReadAsync<T>(IJSRuntime js, string area, string key, T? fallback)
    {
        var json = await js.InvokeAsync<string?>("gatherwellStorage.read", area, key);
        if (string.IsNullOrWhiteSpace(json)) return fallback;
        try { return JsonSerializer.Deserialize<T>(json, JsonOptions) ?? fallback; }
        catch (JsonException) { return fallback; }
    }

    private static Task WriteAsync<T>(IJSRuntime js, string area, string key, T value) =>
        js.InvokeVoidAsync("gatherwellStorage.write", area, key, JsonSerializer.Serialize(value, JsonOptions)).AsTask();
}

public sealed class EventDetails
{
    [Required, StringLength(64, MinimumLength = 2)]
    public string Title { get; set; } = "The Sunday Table";

    [Required, DataType(DataType.Date)]
    public string Date { get; set; } = "2026-11-08";

    [Required]
    public string Time { get; set; } = "11:30";

    [Required, StringLength(90, MinimumLength = 2)]
    public string Venue { get; set; } = "Fern & Field, Brooklyn";

    [Range(1, 500)]
    public int Capacity { get; set; } = 48;

    [StringLength(180)]
    public string Description { get; set; } = "A long lunch for good food, new friends, and the pleasure of taking your time.";
}

public sealed class Attendee
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTimeOffset RegisteredAt { get; set; } = DateTimeOffset.UtcNow;
    public bool CheckedIn { get; set; }
}

public sealed record UserSession(string Name);

public sealed class RegistrationForm
{
    [Required, StringLength(80, MinimumLength = 2)]
    public string Name { get; set; } = "";

    [Required, EmailAddress, StringLength(254)]
    public string Email { get; set; } = "";

    public bool Confirmed { get; set; }
}
