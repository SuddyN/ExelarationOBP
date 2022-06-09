using ExelarationOBPAPI;
using Serilog;

// Set up logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
Log.Information("Starting up");

var builder = WebApplication.CreateBuilder(args);

// Log activity to the console using Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(ctx.Configuration));

// Configure builder services and application pipeline using methods in HostingExtensions.cs
WebApplication? app = builder.ConfigureServices().ConfigurePipeline();

// Set authorization policy for all controllers
//app.MapControllers().RequireAuthorization("ApiScope");

app.Run();
