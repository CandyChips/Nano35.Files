using System;

namespace Nano35.Files.Processor.Models
{
    public class ProfileImageFile :
        ImageFile
    {
        // Data
        public Guid WorkerId { get; set; }
    }
}