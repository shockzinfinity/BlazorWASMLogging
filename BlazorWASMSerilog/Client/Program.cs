using BlazorWASMSerilog.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var logLevelSwitch = new LoggingLevelSwitch();
Log.Logger = new LoggerConfiguration()
  .MinimumLevel.ControlledBy(logLevelSwitch)
  .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
  .WriteTo.BrowserHttp($"{builder.HostEnvironment.BaseAddress}ingest", controlLevelSwitch: logLevelSwitch)
  .WriteTo.BrowserConsole()
  .CreateLogger();

builder.Logging.AddProvider(new SerilogLoggerProvider());

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
