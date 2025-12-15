using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NeoTask.WebApp.Client.Pages;
using NeoTask.WebApp.Components;
using NeoTask.WebApp.Components.Account;
using NeoTask.WebApp.Components.Customization;
using NeoTask.WebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

// Increase SignalR timeout to prevent frequent disconnections
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(30); })
    .AddHubOptions(options =>
    {
        options.ClientTimeoutInterval = TimeSpan.FromMinutes(30);
        options.KeepAliveInterval = TimeSpan.FromSeconds(15);
        options.HandshakeTimeout = TimeSpan.FromSeconds(15);
    });

builder.Services.AddScoped<ThemeState>();
builder.Services.AddScoped<NeoTask.WebApp.Services.IScrollInfoService, NeoTask.WebApp.Services.ScrollInfoService>();
builder.Services.AddSingleton<NeoTask.WebApp.Services.Tasks.Queries.ITaskQueryService, NeoTask.WebApp.Services.Tasks.Queries.TaskQueryService>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddMicrosoftAccount(options =>
    {
        var microsoftAuthConfig = builder.Configuration.GetSection("Authentication:Microsoft");
        options.ClientId = microsoftAuthConfig["ClientId"] ?? throw new InvalidOperationException("Microsoft ClientId not configured.");
        options.ClientSecret = microsoftAuthConfig["ClientSecret"] ?? throw new InvalidOperationException("Microsoft ClientSecret not configured.");
    })
    .AddGoogle(options =>
    {
        var googleAuthConfig = builder.Configuration.GetSection("Authentication:Google");
        options.ClientId = googleAuthConfig["ClientId"] ?? throw new InvalidOperationException("Google ClientId not configured.");
        options.ClientSecret = googleAuthConfig["ClientSecret"] ?? throw new InvalidOperationException("Google ClientSecret not configured.");
    })
    .AddIdentityCookies();

builder.AddNpgsqlDbContext<PostgresDbContext>("neotaskdb");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddEntityFrameworkStores<PostgresDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(NeoTask.WebApp.Client._Imports).Assembly);

app.MapDefaultEndpoints();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
