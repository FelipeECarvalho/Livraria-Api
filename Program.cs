using Livraria;
using Livraria.Data;
using Livraria.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
LoadConfiguration(builder);

ConfigureMvc(builder);

ConfigureAuthentication(builder);

LoadServices(builder);

var app = builder.Build();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();


void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

    builder.Services.AddAuthentication(x => 
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x => 
    {
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}

void ConfigureMvc(WebApplicationBuilder builder)
{
    builder
    .Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
}

void LoadConfiguration(WebApplicationBuilder builder)
{
    Configuration.ConnectionString = builder.Configuration.GetValue<string>("ConnectionString");
    Configuration.JwtKey = builder.Configuration.GetValue<string>("JwtKey");
}

void LoadServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<LivrariaDataContext>();
    builder.Services.AddTransient<TokenService>();

    builder.Services.AddControllersWithViews()
        .AddNewtonsoftJson(options => options
            .SerializerSettings
            .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
}
