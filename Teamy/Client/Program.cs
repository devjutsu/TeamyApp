using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Teamy.Client;
using Teamy.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddHttpClient("private", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddHttpClient("public", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("private"));
builder.Services.AddApiAuthorization()
                    .AddAccountClaimsPrincipalFactory<CustomUserFactory>();

builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<IManageEvents, EventService>();
builder.Services.AddScoped<IManageInvites, InviteService>();
builder.Services.AddScoped<IManagePolls, PollService>();
builder.Services.AddScoped<IManageTemplates, TemplateService>();
builder.Services.AddScoped<IManageUploads, UploadService>();
builder.Services.AddScoped<IManageChats, ChatService>();
builder.Services.AddScoped<IManageQuiz, QuizService>();
builder.Services.AddScoped<IManageAlternatives, AlternativesSerice>();


builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredToast();

await builder.Build().RunAsync();
