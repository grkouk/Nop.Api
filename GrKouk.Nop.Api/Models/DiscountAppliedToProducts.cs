﻿using System;
using System.Collections.Generic;

namespace GrKouk.Nop.Api.Models
{
    public partial class DiscountAppliedToProducts
    {
        public int DiscountId { get; set; }
        public int ProductId { get; set; }

        public virtual Discount Discount { get; set; }
        public virtual Product Product { get; set; }
    }
}
