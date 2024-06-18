using Microsoft.EntityFrameworkCore;
using FotoKlubasSvetaine.Server.Models;

namespace FotoKlubasSvetaine.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Narys> Narys { get; set; }
        public DbSet<Administratorius> Administratoriai { get; set; }
        public DbSet<Klubas> Klubai { get; set; }
        public DbSet<Fotografija> Fotografija { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Narys>(entity =>
            {
                entity.HasKey(e => new { e.NarysID, e.KlubasID });

                entity.HasOne(n => n.Klubas)
                      .WithMany()
                      .HasForeignKey(n => n.KlubasID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(n => n.Administratorius)
                      .WithMany()
                      .HasForeignKey(n => n.Administratorius_AdministratoriusID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Fotografija>(entity =>
            {
                entity.HasKey(e => new { e.FotoID, e.NarysID, e.KlubasID });

                entity.HasOne(f => f.Narys)
                      .WithMany()
                      .HasForeignKey(f => new { f.NarysID, f.KlubasID })
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.Klubas)
                      .WithMany()
                      .HasForeignKey(f => f.KlubasID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Klubas>(entity =>
            {
                entity.HasKey(e => e.KlubasID);
            });

            modelBuilder.Entity<Administratorius>(entity =>
            {
                entity.HasKey(e => e.AdministratoriusID);
            });
        }
    }
}
