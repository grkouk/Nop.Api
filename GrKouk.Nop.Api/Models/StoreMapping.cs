﻿using System;
using System.Collections.Generic;

namespace GrKouk.Nop.Api.Models
{
    public partial class StoreMapping
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public int StoreId { get; set; }

        public virtual Store Store { get; set; }
    }
}
