using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Entities;
using Dansnom.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dansnom.Context
{
    public class DansnomApplicationContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Chat>()
            //             .HasOne(afc => afc.Sender)
            //             .WithMany()
            //             .HasForeignKey(afc => afc.SenderId)
            //             .HasConstraintName("SenderId");
            //             // .WillCascadeOnDelete(true);

            // modelBuilder.Entity<Chat>()
            //             .HasOne(afc => afc.Receiver)
            //             .WithMany()
            //             .HasForeignKey(afc => afc.ReceiverId)
            //             .HasConstraintName("ReceiverId");
                        // .WillCascadeOnDelete(false);
        }
        public DansnomApplicationContext(DbContextOptions<DansnomApplicationContext> options) : base(options)
        {

        }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<ProductOrders> ProductOrders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<ProductionRawMaterial> ProductionRawMaterials { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Like> Likes { get; set; }
    }
}