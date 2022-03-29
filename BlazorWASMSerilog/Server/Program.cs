using Serilog;

Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateBootstrapLogger();

Log.Information("Starting up...");

try {
  var builder = WebApplication.CreateBuilder(args);
  builder.Host.UseSerilog((ctx, lc) => lc
                                      .Enrich.WithCorrelationId()
                                      .Enrich.WithMachineName()
                                      .Enrich.WithProperty("Envrionment", ctx.HostingEnvironment.EnvironmentName)
                                      .Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName)
                                      .Enrich.WithClientIp()
                                      .Enrich.WithClientAgent()
                                      .WriteTo.Console()
                                      .ReadFrom.Configuration(ctx.Configuration));

  // Add services to the container.
  builder.Services.AddHttpContextAccessor();

  builder.Services.AddControllersWithViews();
  builder.Services.AddRazorPages();

  var app = builder.Build();

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment()) {
    app.UseWebAssemblyDebugging();
  } else {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
  }

  app.UseHttpsRedirection();

  app.UseBlazorFrameworkFiles();
  app.UseStaticFiles();

  app.UseSerilogIngestion();
  app.UseSerilogRequestLogging();

  app.UseRouting();

  app.MapRazorPages();
  app.MapControllers();
  app.MapFallbackToFile("index.html");

  app.Run();
} catch (Exception ex) {
  Log.Fatal(ex, "Unhandled exception");
}
finally {
  Log.Information("Shut down complete.");
  Log.CloseAndFlush();
}
