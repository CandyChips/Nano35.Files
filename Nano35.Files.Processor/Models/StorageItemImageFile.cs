using System;

namespace Nano35.Files.Processor.Models
{
    public class StorageItemImageFile :
        ImageFile
    {
        // Data
        public Guid StorageItemId { get; set; }

    }
}