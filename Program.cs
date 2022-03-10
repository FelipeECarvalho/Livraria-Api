using Livraria;
using Livraria.Data;
using Livraria.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

LoadConfiguration(builder);

ConfigureAuthentication(builder);

ConfigureMvc(builder);

LoadServices(builder);

var app = builder.Build();

app.MapControllers();

app.UseAuthentication();

app.UseResponseCompression();

app.UseAuthorization();

app.UseStaticFiles();

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

    builder.Services.AddControllersWithViews()
        .AddNewtonsoftJson(options => options
            .SerializerSettings
            .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
}

void LoadConfiguration(WebApplicationBuilder builder)
{
    Configuration.ConnectionString = builder.Configuration.GetValue<string>("ConnectionString");
    Configuration.JwtKey = builder.Configuration.GetValue<string>("JwtKey");

    var smtp = new Configuration.SmtpConfiguration();

    builder.Configuration.GetSection("Smtp").Bind(smtp);

    Configuration.Smtp = smtp;
}

void LoadServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<LivrariaDataContext>();
    builder.Services.AddTransient<TokenService>();
    builder.Services.AddTransient<EmailService>();
    builder.Services.AddMemoryCache();

    builder.Services.AddResponseCompression(x =>
    {
        x.Providers.Add<GzipCompressionProvider>();
    });

    builder.Services.Configure<GzipCompressionProviderOptions>(options => 
    {
        options.Level = CompressionLevel.Optimal;
    });
}
