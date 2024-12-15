using System;
using drunkShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace drunkShop.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public readonly IConfiguration Configuration;


        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("PostgreConnect"));
        }

        public DbSet<Category> Category { get; set; }

        public DbSet<ApplicationType> ApplicationType { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<AppOrder> AppOrder { get; set; }

        public DbSet<AppOrderProduct> AppOrderProduct { get; set; }

        public DbSet<AppOrderUser> AppOrderUser { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Связь между AppOrder и Product через AppOrderProduct
            modelBuilder.Entity<AppOrderProduct>()
                .HasOne(aop => aop.AppOrder)
                .WithMany()
                .HasForeignKey(aop => aop.AppOrderId);

            modelBuilder.Entity<AppOrderProduct>()
                .HasOne(aop => aop.Product)
                .WithMany()
                .HasForeignKey(aop => aop.ProductId);

            // Связь между AppOrder и User через AppOrderUser
            modelBuilder.Entity<AppOrderUser>()
                .HasOne(aou => aou.AppOrder)
                .WithMany()
                .HasForeignKey(aou => aou.AppOrderId);

            modelBuilder.Entity<AppOrderUser>()
                .HasOne(aou => aou.User)
                .WithMany()
                .HasForeignKey(aou => aou.UserId);
        }
    }
}