using Backend_BankingApp.Model;
using BankingAppBackend.Model;
using Microsoft.EntityFrameworkCore;

namespace BankingAppBackend.DAL
{
    public class BankDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<RequestModel> Requests { get; set; }
        public DbSet<HistoryTransfer> HistoryTransfers { get; set; }

        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
             {
                 base.OnModelCreating(modelBuilder);

                    modelBuilder.Entity<Card>().HasKey(c => c.Id);
                    modelBuilder.Entity<User>().HasKey(u => u.Id);
                    modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
                    modelBuilder.Entity<RequestModel>().HasKey(r => r.Id);
                    modelBuilder.Entity<HistoryTransfer>().HasKey(r => r.Id);


               modelBuilder.Entity<Card>()
                 .HasOne(c => c.User)
                 .WithMany()
                 .HasForeignKey(c => c.OwnerId)
                 .OnDelete(DeleteBehavior.Cascade);

               modelBuilder.Entity<RequestModel>()
                   .HasOne(r => r.Sender)
                   .WithMany()
                   .HasForeignKey(r => r.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

               modelBuilder.Entity<RequestModel>()
                   .HasOne(r => r.Receiver)
                   .WithMany()
                   .HasForeignKey(r => r.ReceiverId)
                   .OnDelete(DeleteBehavior.Restrict);

               modelBuilder.Entity<HistoryTransfer>()
                  .HasOne(ht => ht.UserSender)
                  .WithMany(u => u.SentTransfers)  // Maps to the SentTransfers collection in User
                  .HasForeignKey(ht => ht.IdUserSender)
                  .OnDelete(DeleteBehavior.Restrict);

               modelBuilder.Entity<HistoryTransfer>()
                   .HasOne(ht => ht.UserReceiver)
                   .WithMany(u => u.ReceivedTransfers)  // Maps to the ReceivedTransfers collection in User
                   .HasForeignKey(ht => ht.IdUserReceiver)
                   .OnDelete(DeleteBehavior.Restrict);

               // Configurare pentru TransactionInfo ca obiect owned
               modelBuilder.Entity<Transaction>()
                   .OwnsOne(t => t.Info, ti =>
                   {
                        ti.Property(ti => ti.SenderId).IsRequired();
                        ti.HasOne(ti => ti.Sender)
               .WithMany()
               .HasForeignKey(ti => ti.SenderId)
               .OnDelete(DeleteBehavior.Restrict);

                        ti.HasOne(ti => ti.Receiver)
               .WithMany()
               .HasForeignKey(ti => ti.ReceiverId)
               .OnDelete(DeleteBehavior.Restrict);
                   });
               // Seed pentru UserDetails
               //modelBuilder.Entity<User>().HasData(
               //    new User
               //    {
               //        Id = "1",
               //        Email = "admin@example.com",
               //        FullName = "Admin User",
               //        PasswordHash = Hasher.HashPassword(, // Ar trebui să fie hashed
               //        Phone = "0000000000",
               //        Role = "Admin",
               //        Language = "EN",
               //        SpendingLimit = 10000,
               //        TotalBalance = 50000,
               //        BirthDate = "1990-01-01",
               //        HistoryTransfers = new List<RecentUserSimple>()
               //    }
               //);
          }
    }
}

