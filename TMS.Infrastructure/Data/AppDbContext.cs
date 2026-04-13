using Microsoft.EntityFrameworkCore;
using TMS.Core.Entities;

namespace TMS.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Terminal> Terminals => Set<Terminal>();
    public DbSet<Command> Commands => Set<Command>();
    public DbSet<CommandLog> CommandLogs => Set<CommandLog>();
    public DbSet<Site> Sites => Set<Site>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();

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

            // Relation Site → Terminals : FK nullable, SetNull si le site est supprimé
            entity.HasOne(t => t.Site)
                  .WithMany(s => s.Terminals)
                  .HasForeignKey(t => t.SiteId)
                  .OnDelete(DeleteBehavior.SetNull)
                  .IsRequired(false);

            // Relation Warehouse → SpareTerminals : FK nullable, SetNull si la warehouse est supprimée
            entity.HasOne(t => t.Warehouse)
                  .WithMany(w => w.SpareTerminals)
                  .HasForeignKey(t => t.WarehouseId)
                  .OnDelete(DeleteBehavior.SetNull)
                  .IsRequired(false);
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

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Name).IsRequired().HasMaxLength(100);
            entity.Property(w => w.Address).HasMaxLength(200);
            entity.HasIndex(w => w.Name);
        });

        modelBuilder.Entity<Site>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Address).HasMaxLength(200);
            entity.HasIndex(s => s.Name);

            // Restrict : on ne peut pas supprimer une warehouse qui a encore des sites
            entity.HasOne(s => s.Warehouse)
                  .WithMany(w => w.Sites)
                  .HasForeignKey(s => s.WarehouseId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
