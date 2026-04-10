using Microsoft.EntityFrameworkCore;
using TMS.Core.Entities;

namespace TMS.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Terminal> Terminals => Set<Terminal>();
    public DbSet<Command> Commands => Set<Command>();
    public DbSet<CommandLog> CommandLogs => Set<CommandLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Terminal>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
            entity.Property(t => t.SerialNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(t => t.SerialNumber).IsUnique();
            entity.Property(t => t.Model).HasMaxLength(100);
            entity.Property(t => t.IpAddress).HasMaxLength(45);
            entity.Property(t => t.Location).HasMaxLength(200);
        });

        modelBuilder.Entity<Command>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasOne(c => c.Terminal)
                  .WithMany(t => t.Commands)
                  .HasForeignKey(c => c.TerminalId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommandLog>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.HasOne(l => l.Command)
                  .WithMany(c => c.Logs)
                  .HasForeignKey(l => l.CommandId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
