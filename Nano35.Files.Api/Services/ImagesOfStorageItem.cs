using System;
using Microsoft.EntityFrameworkCore;

namespace Nano35.Files.Api.Services
{
    public class ImagesOfStorageItem
    {
        public Guid StorageItemId { get; set; }
        public DateTime Uploaded { get; set; }
        public bool IsConfirmed { get; set; }
    }

    public class ImagesOfStorageItemFluentContext
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            // Primary key
            modelBuilder.Entity<ImagesOfStorageItem>()
                .HasKey(u => new {u.StorageItemId});  
            
            // Data
            modelBuilder.Entity<ImagesOfStorageItem>()
                .Property(b => b.Uploaded)
                .IsRequired();
            modelBuilder.Entity<ImagesOfStorageItem>()
                .Property(b => b.IsConfirmed)
                .IsRequired();
            
            // Foreign keys
        }
    }
}