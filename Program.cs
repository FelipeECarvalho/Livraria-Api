using Livraria;
using Livraria.Data;

var builder = WebApplication.CreateBuilder(args);
LoadServices(builder);
ConfigureMvc(builder);
var app = builder.Build();

app.MapControllers();

LoadConfiguration(app);

app.Run();


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

void LoadConfiguration(WebApplication app)
{
    Configuration.ConnectionString = app.Configuration.GetValue<string>("ConnectionString");
}

void LoadServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    builder.Services.AddDbContext<LivrariaDataContext>();

    builder.Services.AddControllersWithViews()
        .AddNewtonsoftJson(options => options
            .SerializerSettings
            .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
}
