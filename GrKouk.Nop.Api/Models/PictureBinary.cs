using System;
using System.Collections.Generic;

namespace GrKouk.Nop.Api.Models
{
    public partial class PictureBinary
    {
        public int Id { get; set; }
        public byte[] BinaryData { get; set; }
        public int PictureId { get; set; }

        public virtual Picture Picture { get; set; }
    }
}
