using LightOn.Data;
using LightOn.Models;
using LightOn.Repositories;
using LightOn.Repositories.Interfaces;
using LightOn.Services;
using LightOn.Services.Interfaces;
using LightOn.Websockets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

var connectionString = builder.Configuration.GetConnectionString("LightsOnDb");
builder.Services.AddSingleton<ILoggingService, LoggingService>();
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<User, Role>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)    
     .AddJwtBearer(options =>
     {
         options.SaveToken = true;
         options.RequireHttpsMetadata = false;
         options.TokenValidationParameters = new TokenValidationParameters()
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidAudience = builder.Configuration["JWT:ValidAudience"],
             ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
             ClockSkew = TimeSpan.FromMinutes(10),
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Secret").Value))
         };
     })
     .AddGoogle(googleOptions =>
     {
         googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
         googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
     });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdminPolicy",
        policy => policy.RequireAssertion(context => context.User.HasClaim(c => (c.Type == "IsAdmin" && c.Value == "true"))));
});

builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("DistCache_ConnectionString");
    options.SchemaName = "dbo";
    options.TableName = "CachedValues";
});

builder.Services.AddScoped<IApplianceRepository, ApplianceRepository>();
builder.Services.AddScoped<IApplianceUsageRepository, ApplianceUsageRepository>();
builder.Services.AddScoped<IApplianceUsagePlannedRepository, ApplianceUsagePlannedRepository>();
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
builder.Services.AddScoped<IDistrictRepository, DistrictRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IStreetRepository, StreetRepository>();
builder.Services.AddScoped<ITownRepository, TownRepository>();
builder.Services.AddScoped<ITransformerMeasurementRepository, TransformerMeasurementRepository>();
builder.Services.AddScoped<ITransformerRepository, TransformerRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();

builder.Services.AddScoped<IApplianceService, ApplianceService>();
builder.Services.AddScoped<IApplianceUsageService, ApplianceUsageService>();
builder.Services.AddScoped<IApplianceUsagePlannedService, ApplianceUsagePlannedService>();
builder.Services.AddScoped<IBuildingService, BuildingService>();
builder.Services.AddScoped<IDistrictService, DistrictService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IStreetService, StreetService>();
builder.Services.AddScoped<ITownService, TownService>();
builder.Services.AddScoped<ITransformerMeasurementService, TransformerMeasurementService>();
builder.Services.AddScoped<ITransformerService, TransformerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseWebSockets();
app.Map("/ws", HandleWebSocketRequest);

app.Run();

void HandleWebSocketRequest(IApplicationBuilder app)
{
    app.Use(async (context, next) =>
    {
        if (context.Request.PathBase == "/ws")
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                int transformerId = GetTransformerIdFromContext(context); // Replace with your logic to retrieve the transformer ID

                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await WebSocketHandler.HandleWebSocketConnection(webSocket, transformerId);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }
        else
        {
            await next();
        }
    });
}

int GetTransformerIdFromContext(HttpContext context)
{
    // Implement your logic to extract the transformer ID from the context
    // You can retrieve it from request headers, query parameters, or any other source
    // For this example, assume the transformer ID is passed as a query parameter named "id"

    if (int.TryParse(context.Request.Query["id"], out int transformerId))
    {
        return transformerId;
    }

    throw new ArgumentException("Invalid transformer ID");
}