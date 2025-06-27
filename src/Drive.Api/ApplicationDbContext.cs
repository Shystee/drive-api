using Drive.Api.Domain;
using Microsoft.EntityFrameworkCore;
using File = Drive.Api.Domain.File;

namespace Drive.Api;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<File> Files => Set<File>();
	
	public DbSet<Album> Albums => Set<Album>();
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
	}
}