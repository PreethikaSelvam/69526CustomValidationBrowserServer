using CustomValidationBrowserServer.Client.Pages;
using CustomValidationBrowserServer.Components;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddValidation();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

var spanishCulture = new CultureInfo("es-ES");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(spanishCulture),
    SupportedCultures = [spanishCulture],
    SupportedUICultures = [spanishCulture]
});

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(CustomValidationBrowserServer.Client._Imports).Assembly);

app.Run();
