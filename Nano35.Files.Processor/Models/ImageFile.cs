using System;
using Microsoft.EntityFrameworkCore;

namespace Nano35.Files.Processor.Models
{
    public class ImageFile
    {
        public Guid Id { get; set; }
        public string Format  { get; set; }
        public string Name  { get; set; }
    }

    public class FluentContext
    {
        public static void Client(ModelBuilder modelBuilder)
        {
            //Primary key
            modelBuilder.Entity<ImageFile>()
                .HasKey(u => new {u.Id});
            
            //Data
            modelBuilder.Entity<ImageFile>()
                .Property(b => b.Format)    
                .HasColumnType("nvarchar(MAX)")
                .IsRequired();
            modelBuilder.Entity<ImageFile>()
                .Property(b => b.Name)    
                .HasColumnType("nvarchar(MAX)")
                .IsRequired();
        }
    }
}