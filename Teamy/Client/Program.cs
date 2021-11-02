using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Teamy.Client;
using Teamy.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Teamy.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
builder.Services.AddHttpClient("Teamy.AnonymousAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Teamy.ServerAPI"));


builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<IManageEvents, EventService>();
builder.Services.AddScoped<IManageInvites, InviteService>();
builder.Services.AddScoped<IManagePolls, PollService>();
builder.Services.AddScoped<IManageTemplates, TemplateService>();
builder.Services.AddScoped<IManageUploads, UploadService>();

//builder.Services.AddApiAuthorization()
//                    .AddAccountClaimsPrincipalFactory<CustomUserFactory>();
//builder.Services.AddBlazoredModal();

builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();
