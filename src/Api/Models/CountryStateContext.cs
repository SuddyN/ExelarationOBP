using Microsoft.EntityFrameworkCore;
namespace Api.Models;

public class CountryStateContext: DbContext {
    public CountryStateContext(DbContextOptions<CountryStateContext> options) : base(options) { }
    public DbSet<Country> Countrys { get; set; } = null!;
    public DbSet<State> States { get; set; } = null!;
}
