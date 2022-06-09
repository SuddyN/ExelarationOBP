using ExelarationOBPAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace ExelarationOBPAPI;

internal static class HostingExtensions {
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder) {
        // Add services to the container.
        builder.Services.AddRazorPages(); // UI
        builder.Services.AddControllers(); // API Controllers
        builder.Services.AddDbContext<CountryStateContext>(opt =>
            opt.UseInMemoryDatabase("CountryState")); // Database
        builder.Services.AddEndpointsApiExplorer();

        // IdentityServer Service
        builder.Services.AddIdentityServer(options => {
            // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
            options.EmitStaticAudienceClaim = true;
        })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients);

        // API Authentication policy
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options => {
                // Listen on this address
                options.Authority = "https://localhost:7255";
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateAudience = false // Audience validation is disabled because API access is modeled with ApiScopes only.
                };
            });


        // API Authorization policy:
        // The protocol ensures that this scope will only be in the token if the
        // client requests it and IdentityServer allows the client to have that scope.
        builder.Services.AddAuthorization(options => {
            options.AddPolicy("ApiScope", policy => {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "countryState");
            });
        });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app) {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }

        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();
        app.UseStaticFiles(); // UI
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers(); // API Controllers
        app.MapRazorPages(); // UI
        //app.MapRazorPages().RequireAuthorization();
        //app.UseRouting();
        app.UseIdentityServer();

        return app;
    }
}
