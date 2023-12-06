using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;

namespace DAL;

public sealed class AppDataContext : DbContext
{
	public AppDataContext()
	{
		
	}

	public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
	{
		
	}

	public DbSet<AppUser> AppUsers { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder
			.ApplyConfigurationsFromAssembly(typeof(AppDataContext).Assembly);
	}
}