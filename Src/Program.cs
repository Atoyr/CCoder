using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using ElectronNET.API;
using ElectronNET.API.Entities;

using CCoder.Data;
using CCoder.Dotnet;
using CCoder.Provider;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<IConfigProvider, ConfigProvider>();
builder.Services.AddSingleton<Config>();
builder.WebHost.UseElectron(args);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        Console.WriteLine($"user profile : {userProfile}");
        string configPath =  Path.Combine(userProfile, ".config");
        if (!Directory.Exists(configPath))
        {
            Directory.CreateDirectory(configPath);
        }

        string appConfigPath = Path.Combine(configPath, "CCoder");
        if (!Directory.Exists(appConfigPath))
        {
            Directory.CreateDirectory(appConfigPath);
        }

foreach(var s in DotnetProcess.Process.GetDotnetVersions())
{
    Console.WriteLine(s);
}

// DotnetProcess.Process.CreateProject(@"C:\Users\r.uchiyama\src\github.com\atoyr\echo");
var pr = DotnetProcess.Process.RunAsync(@"C:\Users\r.uchiyama\src\github.com\atoyr\echo", new string[]{"hogehoge"});
foreach(var s in pr.Outputs)
{
    Console.WriteLine(s);
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

Electron.Menu.SetApplicationMenu(new MenuItem[]{});
Task.Run(async () => await Electron.WindowManager.CreateWindowAsync());
app.Run();
