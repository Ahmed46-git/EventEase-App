# Gatherwell Event Studio (Blazor)

A responsive .NET 10 Blazor Web App for event planning, RSVP registration, and attendance check-in. The app uses server interactivity and stores event details and guests in this browser's local storage; the sign-in demo uses session storage and is not production authentication.

## Run locally

From this directory, run `dotnet run` and open the HTTPS or HTTP URL printed by the app.

## Development steps and Copilot assistance

1. **Create the separate project.** Scaffolded a .NET 10 Blazor Web App under `event-studio/`, beside and independent from `portfolio/`. Copilot helped establish the routed, interactive Blazor foundation.
2. **Build the Event Card.** Added a typed event model, input validation, and a live card editor. Razor two-way binding connects the editable form and preview. Copilot helped generate the initial component and binding structure.
3. **Implement and debug routing.** Added Blazor routes for Overview, Event card, Registration, and Attendance, shared navigation, and a not-found view. Copilot helped check the route boundaries and fallback behavior.
4. **Optimize and validate.** Added bounded validation, duplicate and capacity checks, browser storage interop, and scoped components. Copilot helped identify validation and persistence edge cases; `dotnet build` checks compilation.
5. **Add advanced workflows.** Implemented registration, tab-scoped session display, searchable guest list, check-in, and guest removal. Copilot helped connect workflows to shared scoped state.
6. **Test the user flows.** Edit both sides of the card, save valid and invalid details, register an attendee, attempt a duplicate email, check them in, refresh, and visit an unknown path. Copilot helped outline manual acceptance checks.

## GitHub repository

This project is local and has not been published as a GitHub repository. Create a repository named `gatherwell-event-studio` on GitHub and connect this folder to publish it. Choose public or private based on course requirements and do not publish real attendee information.
