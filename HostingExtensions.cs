using ExelarationOBPAPI.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExelarationOBPAPI;

internal static class HostingExtensions {
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder) {
        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<CountryStateContext>(opt =>
            opt.UseInMemoryDatabase("CountryState"));
        builder.Services.AddEndpointsApiExplorer();

        // IdentityServer Service
        builder.Services.AddIdentityServer(options => {
            // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
            options.EmitStaticAudienceClaim = true;
        })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients);

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app) {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }

        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAuthorization();
        app.MapControllers();
        app.MapRazorPages();
        //app.MapRazorPages().RequireAuthorization();
        //app.UseRouting();
        app.UseIdentityServer();

        return app;
    }
}
