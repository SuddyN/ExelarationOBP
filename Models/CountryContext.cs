using Microsoft.EntityFrameworkCore;
namespace ExelarationOBPAPI.Models;

public class CountryContext: DbContext {
    public CountryContext(DbContextOptions<CountryContext> options) : base(options) { }
    public DbSet<Country> Countrys { get; set; } = null!;
}
