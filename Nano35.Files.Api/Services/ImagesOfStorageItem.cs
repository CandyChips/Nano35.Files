using System;
using Microsoft.EntityFrameworkCore;

namespace Nano35.Files.Api.Services
{
    public class ImagesOfStorageItem
    {
        public Guid Id { get; set; }
        public Guid StorageItemId { get; set; }
        public DateTime Uploaded { get; set; }
        public string RealName { get; set; }
        public string NormalizedName { get; set; }
        public bool IsConfirmed { get; set; }
    }

    public class ImagesOfStorageItemFluentContext
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImagesOfStorageItem>()
                .HasKey(u => new {u.Id});  
            
            modelBuilder.Entity<ImagesOfStorageItem>()
                .Property(b => b.StorageItemId)
                .IsRequired();
            modelBuilder.Entity<ImagesOfStorageItem>()
                .Property(b => b.Uploaded)
                .IsRequired();
            modelBuilder.Entity<ImagesOfStorageItem>()
                .Property(b => b.IsConfirmed)
                .IsRequired();
            modelBuilder.Entity<ImagesOfStorageItem>()
                .Property(b => b.RealName)
                .IsRequired();
            modelBuilder.Entity<ImagesOfStorageItem>()
                .Property(b => b.NormalizedName)
                .IsRequired();
        }
    }
}