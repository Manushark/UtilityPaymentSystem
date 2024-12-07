using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using UtilityPaymentSystem.Domain.Entities;


namespace UtilityPaymentSystem.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Report> Reports { get; set; }
        
    

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Bill>()
                    .HasOne(b => b.Service)
                    .WithMany()
                    .HasForeignKey(b => b.ServiceId);
            }
            
        

    }

}

