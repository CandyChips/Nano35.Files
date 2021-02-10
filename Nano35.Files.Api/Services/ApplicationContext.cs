using Microsoft.EntityFrameworkCore;

namespace Nano35.Files.Api.Services
{
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new ImagesOfStorageItemFluentContext().Configure(modelBuilder);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
