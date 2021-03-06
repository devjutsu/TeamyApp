using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using Teamy.Server.Data;
using Teamy.Server.Logic;
using Teamy.Server.Models;
using Teamy.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TeamyDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 7;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TeamyDbContext>()
    .AddClaimsPrincipalFactory<AppClaimsPrincipalFactory>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<AppUser, TeamyDbContext>(options => {
        options.IdentityResources["openid"].UserClaims.Add("name");
        options.ApiResources.Single().UserClaims.Add("name");
        options.IdentityResources["openid"].UserClaims.Add("role");
        options.ApiResources.Single().UserClaims.Add("role");
        options.IdentityResources["openid"].UserClaims.Add("DisplayName");
        options.ApiResources.Single().UserClaims.Add("DisplayName");
    });
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

builder.Services.AddAuthentication()
    .AddTwitter(twitterOptions =>
    {
       twitterOptions.ConsumerKey = builder.Configuration["Integrations:Twitter:ApiKey"];
       twitterOptions.ConsumerSecret = builder.Configuration["Integrations:Twitter:ApiKeySecret"];
       twitterOptions.RetrieveUserDetails = true;
       twitterOptions.SaveTokens = true;
       twitterOptions.ClaimActions.MapJsonKey("display-name", "name");
       twitterOptions.ClaimActions.MapJsonKey("profile-image-url", "profile_image_url_https");
    })
    //.AddFacebook(facebookOptions =>
    //{
    //    facebookOptions.AppId = builder.Configuration["Integrations:Facebook:ClientId"];
    //    facebookOptions.AppSecret = builder.Configuration["Integrations:Facebook:ClientSecret"];
    //    facebookOptions.SaveTokens = true;
    //    facebookOptions.ClaimActions.MapJsonKey("display-name", "name");
    //    facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";
    //})
    .AddGoogle(options =>
    {
       IConfigurationSection googleAuthNSection =
           builder.Configuration.GetSection("Integrations:Google");

       options.ClientId = googleAuthNSection["ClientId"];
       options.ClientSecret = googleAuthNSection["ClientSecret"];
    })
    .AddIdentityServerJwt();

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IStorageService, AzureBlobStorageService>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.Configure<BlobStorageOptions>(builder.Configuration.GetSection("Storage"));
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IChatHub, ChatHub>();
builder.Services.AddSingleton<IVoteHub, VoteHub>();
builder.Services.AddTransient<IUploadImages, DbImgUpload>();
builder.Services.AddTransient<IManageEvents, EventLogic>();

//builder.Configuration.AddUserSecrets<Program>();

//builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
    dbInitializer.Initialize();
    dbInitializer.SeedData();
}

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.MapHub<VoteHub>("/votehub");
app.MapHub<ChatHub>("/chathub");

app.Run();
