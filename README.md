# Custom Validation Across Browser and Server

This repository contains the test application and manual evidence for [dotnet/aspnetcore issue #69526](https://github.com/dotnet/aspnetcore/issues/69526): custom validation attributes across the browser and server in an ASP.NET Core Blazor Web App using static server-side rendering (static SSR).

## Test outcome

**Works.** All required behavior and all 16 manual test cases passed. No runtime or documentation defect was identified.

See the complete [test report](./Evidence/CustomValidationAcrossBrowserAndServerReport.docx) and [manual evidence](./Evidence/TestCasesAndOutput).

## Environment tested

- .NET SDK `11.0.100-rtm.26476.107`
- ASP.NET Core runtime `11.0.0-rtm.26476.107`
- Blazor Web App with static SSR
- Spanish (`es-ES`) validation localization
- Windows 11 Enterprise 24H2 x64, build 26100
- Google Chrome
- Visual Studio Code

The repository's [`global.json`](./global.json) selects the tested .NET SDK.

## Scenario

The application contains an event-submission form whose title must contain between 3 and 6 words, inclusive.

The custom `WordCountAttribute`:

- Validates authoritatively on the server.
- Implements `IClientValidationRuleProvider`.
- Emits `minimum` and `maximum` parameters for browser validation.
- Overrides `ValidationAttribute.FormatMessage` to format localized templates.
- Associates server failures with the `Title` field.

A matching JavaScript validator is registered once during application startup. It uses the emitted parameters and allows the emitted rule to supply the localized message.

## Localization

The selected UI culture is Spanish (`es-ES`). The per-model resource contains:

- `EventModel_Title_DisplayName`
- `EventModel_Title_WordCountAttribute_Error`

The translated message intentionally places the maximum before the minimum:

```text
{0} debe tener como máximo {2} palabras y como mínimo {1} palabras.
```

For the configured bounds, the rendered message is:

```text
Título localizado debe tener como máximo 6 palabras y como mínimo 3 palabras.
```

The same localized message is used by browser validation and server validation.

## Build and run

From the repository root:

```powershell
dotnet --info
dotnet restore .\CustomValidationBrowserServer.sln
dotnet build .\CustomValidationBrowserServer.sln --no-restore
dotnet run --project .\CustomValidationBrowserServer\CustomValidationBrowserServer.csproj --no-build --launch-profile http
```

Open:

```text
http://localhost:5183/
```

Keep the terminal running while testing.

## Core manual test

1. Open browser Developer Tools and select **Network**.
2. Enable **Preserve log** and **Disable cache**.
3. Start at `/`, which contains no form.
4. Follow **Abrir el formulario de eventos** using enhanced navigation.
5. Enter `Festival plan` and leave the field.
6. Verify that the localized browser message appears without a POST.
7. Enter `Uno dos tres cuatro cinco seis siete`.
8. Verify the above-maximum message, then correct the title to an in-range value
   and confirm that the message clears.
9. Enter `Festival plan` and click the ordinary **Enviar** button.
10. Verify that browser validation blocks the submit without a POST.
11. Click **Enviar sin validación del navegador**.
12. Verify a POST to `/events/new`, server rejection, the same localized field
    message, and no confirmation.
13. Enter `Festival anual de música` and click ordinary **Enviar**.
14. Verify a successful POST and the confirmation:

    ```text
    Evento enviado correctamente.
    ```

## Manual test coverage

The evidence covers:

1. Enhanced navigation and below-minimum browser validation
2. Above-maximum browser validation
3. Correction of invalid input
4. Blocked ordinary invalid submission
5. `formnovalidate` server rejection and message parity
6. Valid ordinary submission
7. Exact minimum boundary
8. Exact maximum boundary
9. Minimum minus one
10. Maximum plus one
11. Repeated-space and tab normalization
12. Invalid whitespace browser/server parity
13. Empty optional value
14. Repeated enhanced navigation
15. Direct form-page loading
16. Browser refresh

Detailed observations and links to each screenshot and recording are in the [test report](./Evidence/CustomValidationAcrossBrowserAndServerReport.docx).

## Build and execution evidence

- [SDK information](./Evidence/Build/dotnet-info.txt)
- [Build output](./Evidence/Build/baseline-build.txt)
- [Application run log](./Evidence/Build/dotnet-run-log.txt)
