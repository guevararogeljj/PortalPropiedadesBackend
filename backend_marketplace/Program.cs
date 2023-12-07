using AspNetCoreRateLimit;
using BusinessLogic;
using DataSource;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;
using LeadsServiceManager = LeadsInjection.ServiceManager;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureAppConfiguration(x =>
{
    x.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", false, true); // add file environment settings
});

Log.Logger = new LoggerConfiguration().CreateBootstrapLogger(); // set new logger in ILogger service
builder.Host.UseSerilog((x, y) => // load configuration logger
{
    y.ReadFrom.Configuration(x.Configuration);
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddHttpClient("propiedades", x =>
//{
//    x.Timeout = TimeSpan.FromSeconds(60);
//});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    options.UseLazyLoadingProxies();

}, ServiceLifetime.Scoped); // add datacontext to app


RepositoryManager.AddRegistration(builder.Services); // add repositories to app
ServiceManager.AddRegistration(builder.Services, builder.Configuration); // services dependency injenction - business logic
LeadsServiceManager.AddRegistration(builder.Services, builder.Configuration); // services dependency injenction  -  injecntio to leads
JWTManager.AddRegistration(builder.Services, builder.Configuration); // add configuration for jwt

var cultureInfo = new CultureInfo("es-MX");
cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
cultureInfo.DateTimeFormat.DateSeparator = "/";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var allowOrigin = builder.Configuration.GetSection("AllowedHostCors").Get<string[]>();

builder.Services.AddCors(options => // add configuration cors 
{
    options.AddDefaultPolicy(builder => builder
    //.SetIsOriginAllowed((x) => true)
    .AllowAnyMethod()
    .WithOrigins(allowOrigin)
    .AllowAnyHeader()
    .AllowCredentials()
     );

    options.AddPolicy("CorsPolicy", builder =>
        builder
     //.SetIsOriginAllowed((x) => true)
     .AllowAnyMethod()
     .WithOrigins(allowOrigin)
     .AllowAnyHeader()
     .AllowCredentials());
});

builder.Services.AddMemoryCache(); // add ratelimite for set max of request by client
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddInMemoryRateLimiting();

builder.Services.AddAntiforgery(options => // configuration for csrf
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "XSRF-COOKIE-TOKEN";
    options.Cookie.HttpOnly = false;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.SuppressXFrameOptionsHeader = false;
    //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = 429;
    options.RealIpHeader = "X-Real-IP";
    options.GeneralRules = new List<RateLimitRule> {
    new RateLimitRule{
        Endpoint="*",
        Period = "3s",
        Limit = 5
    } };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "QA")
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("Strict-Transport-Security", "min-age=31536000");
    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");

    context.Response.Headers.Remove("x-powered-by");
    context.Response.Headers.Remove("X-Powered-By");
    context.Response.Headers.Remove("Server");
    context.Response.Headers.Remove("x-sourcefiles");
    context.Response.Headers.Remove("X-Sourcefiles");
    context.Response.Headers.Remove("x-rate-limit-limit");
    context.Response.Headers.Remove("x-rate-limit-remainning");
    context.Response.Headers.Remove("x-rate-limit-reset");

    await next();
});

app.UseSerilogRequestLogging();

app.UseCors("CorsPolicy");

app.UseCors();

app.UseHttpsRedirection();

app.UseIpRateLimiting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
