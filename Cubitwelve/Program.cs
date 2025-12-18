using Cubitwelve.Src.Extensions;
using Cubitwelve.Src.Middlewares;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var port = Environment.GetEnvironmentVariable("PORT") ?? "80";

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls($"http://*:{port}");

var localAllowSpecificOrigins = "_localAllowSpecificOrigins";
var deployedAllowSpecificOrigins = "_deployedAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: localAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                                .WithOrigins("http://localhost:3000",
                                            "http://localhost:8100",
                                            "http://localhost",
                                            "https://cubi12-front-latest.onrender.com");
                      });
    options.AddPolicy(name: deployedAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                                .WithOrigins("https://cubi12.azurewebsites.net",
                                            "https://cubi12.cl",
                                            "https://www.cubi12.cl"
                                            );
                      });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => builder.Cache());
});

var app = builder.Build();
app.UseOutputCache();


// Because it's the first middleware, it will catch all exceptions
app.UseExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(localAllowSpecificOrigins);
}
else
{
    app.UseCors(deployedAllowSpecificOrigins);
}

app.UseAuthentication();
app.UseAuthorization();

// app.UseIsUserEnabled();

app.UseHttpsRedirection();

app.MapControllers();

// Database Bootstrap
AppSeedService.SeedDatabase(app);

app.Run();