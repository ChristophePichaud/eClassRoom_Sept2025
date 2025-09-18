using Microsoft.EntityFrameworkCore;
using EFModel.Models;

namespace EFModel
{
    public class EClassRoomDbContext : DbContext
    {
        public EClassRoomDbContext(DbContextOptions<EClassRoomDbContext> options)
            : base(options)
        {
        }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<SalleDeFormation> SallesDeFormation { get; set; }
        public DbSet<ProvisionningVM> ProvisionningVMs { get; set; } // Ajout du DbSet pour ProvisionningVM

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProvisionningVM>()
                .HasOne(p => p.SalleDeFormation)
                .WithMany()
                .HasForeignKey(p => p.SalleDeFormationId);

            modelBuilder.Entity<ProvisionningVM>()
                .HasOne(p => p.Stagiaire)
                .WithMany()
                .HasForeignKey(p => p.StagiaireId);
        }
    }
}