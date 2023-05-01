using AuthAPI.Middleware;
using AuthAPI.Services;
using AuthAPI.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

configuration.AddEnvironmentVariables();
// Register AuthDatabaseSettings

var authDbSettingsSection = configuration.GetSection(nameof(AuthDatabaseSettings));
services.Configure<AuthDatabaseSettings>(authDbSettingsSection);

services.AddSingleton<IAuthDatabaseSettings>(sp =>
  sp.GetRequiredService<IOptions<AuthDatabaseSettings>>().Value);

// Register MongoClient
services.AddSingleton<IMongoClient>(serviceProvider => {
  var settings = serviceProvider.GetRequiredService<IOptions<AuthDatabaseSettings>>().Value;
  return new MongoClient(settings.ConnectionString);
});

// Register UserService
services.AddScoped<IUserService, UserService>();

var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
services.AddSingleton(jwtSettings);

services.AddScoped<IJwtService, JwtService>();

// Add authentication configuration

services.AddAuthentication(x =>
  {
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
  })
  .AddJwtBearer(x =>
  {
    x.TokenValidationParameters = new TokenValidationParameters
    {
      IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSettings.SecretKey)),
      ValidateIssuer = false,
      ValidateAudience = false,
      RequireExpirationTime = true,
      ClockSkew = TimeSpan.Zero
    };
  });
services.AddControllers();

services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
// Configure middleware and other settings
app.UseSwagger();
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API V1");
});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseEndpoints(endpoints =>
{
  endpoints.MapControllers();
});
app.Run();
