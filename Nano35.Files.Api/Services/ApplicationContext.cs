using System;
using Microsoft.EntityFrameworkCore;

namespace Nano35.Files.Api.Services
{
    public class ImagesOfStorageItem
    {
        public Guid StorageItemId { get; set; }
        public DateTime Uploaded { get; set; }
        public bool Confirmed { get; set; }
    }
    
    public class ApplicationContext : DbContext
    {
        
        public DbSet<ImagesOfStorageItem> ImagesOfStorageItems {get;set;}
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
            Update();
        }
        
        public void Update()
        {
            ImagesOfStorageItems.Load();
        }
    }
}
