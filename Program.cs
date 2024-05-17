using Codraw.Configurations;
using Codraw.Framework.DbCore;
using Codraw.Models.Team;
using Codraw.Models.User;
using Codraw.Services.AuthService;
using Codraw.Services.GoogleAuthService;
using Codraw.Services.RegistrationServices;
using Codraw.Services.TeamBoardService;
using Codraw.Services.TeamsService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

var key =
    config.GetSection("Jwt").GetSection("Key").Value
    ?? throw new InvalidOperationException("JWT key is missing from the configuration.");

builder
    .Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// ---------------------- Db Services --------------------

var userDbService = CosmosDbInitializer.InitializeCosmosClientInstance<UserDetails>(
    config.GetSection("UsersDb")
);
var teamsDbService = CosmosDbInitializer.InitializeCosmosClientInstance<Team>(
    config.GetSection("TeamsDb")
);
var teamBoardsDbService = CosmosDbInitializer.InitializeCosmosClientInstance<TeamBoard>(
    config.GetSection("TeamBoardsDb")
);


// -------------------------- DI --------------------------

builder.Services.AddSingleton<ICosmosDbService<UserDetails>>(userDbService);
builder.Services.AddSingleton<ICosmosDbService<Team>>(teamsDbService);
builder.Services.AddSingleton<ICosmosDbService<TeamBoard>>(teamBoardsDbService);

builder.Services.AddHttpClient<IGoogleAuthClient, GoogleAuthClient>().SetHandlerLifetime(TimeSpan.FromMinutes(15));
builder.Services.AddSingleton<IConfigManager, ConfigManager>();
builder.Services.AddSingleton<IAuthClient, AuthClient>();
builder.Services.AddSingleton<IGoogleAuthClient, GoogleAuthClient>();
builder.Services.AddSingleton<IRegistrationClient, RegistrationClient>();
builder.Services.AddSingleton<ITeamsClient, TeamsClient>();
builder.Services.AddSingleton<ITeamBoardClient, TeamBoardClient>();

// -------------------------- App -------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
