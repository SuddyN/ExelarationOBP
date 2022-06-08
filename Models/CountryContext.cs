using Microsoft.EntityFrameworkCore;

namespace ExelarationOBPASPDotNET.Models;
public class CountryContext: DbContext {
    public CountryContext(DbContextOptions<CountryContext> options) : base(options) {

    }

    public DbSet<Country> Countrys { get; set; } = null!;
}
