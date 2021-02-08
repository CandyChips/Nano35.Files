using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nano35.Files.Processor.Models;

namespace Nano35.Files.Processor.Services
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ImageFile> ImageFiles { get; set; }
        public DbSet<ProfileImageFile> ProfileImages { get; set; }
        public DbSet<StorageItemImageFile> StorageItemImages {get;set;}
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options) :
            base(options)
        {
            Database.EnsureCreated();
            Update();
        }
        public void Update()
        {
            ImageFiles.Load();
            ProfileImages.Load();
            StorageItemImages.Load();
        }
    }
}
