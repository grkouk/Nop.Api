using System;
using System.Collections.Generic;

namespace GrKouk.Nop.Api.Models
{
    public partial class CategoryBanner
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int CategoryId { get; set; }
        public int PictureId { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
    }
}
