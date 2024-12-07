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
        public DbSet<Service> Service { get; set; }
        public DbSet<Bill> Bill { get; set; }
        public DbSet<Payment> Payment { get; set; }
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

