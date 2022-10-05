using ConfigDashSample.Client;
using ConfigDashSample.Shared;
using ConfigDashSample.Shared.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddTelerikBlazor();
builder.Services.AddSingleton<LocalStorage>();
builder.Services.AddSingleton<TilesDataService>();

await builder.Build().RunAsync();