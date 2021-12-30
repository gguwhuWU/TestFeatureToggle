using Microsoft.FeatureManagement;
using System.Diagnostics;

//var builder = WebApplication.CreateBuilder(args);
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Staging
});

Debug.WriteLine($"Application Name: {builder.Environment.ApplicationName}");
Debug.WriteLine($"Environment Name: {builder.Environment.EnvironmentName}");
Debug.WriteLine($"ContentRoot Path: {builder.Environment.ContentRootPath}");

// Add services to the container.
builder.Services.AddControllersWithViews();

var config = builder.Configuration
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json")
                  .AddJsonFile("appsettings.Development.json")
                  .Build();
foreach (var c in config.AsEnumerable())
{
    Debug.WriteLine(c.Key + " = " + c.Value);
}

var x = config.GetSection("FeatureManagement");
builder.Services.AddFeatureManagement(x);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();