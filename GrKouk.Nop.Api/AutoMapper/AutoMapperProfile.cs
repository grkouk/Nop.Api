using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrKouk.Nop.Api.Dtos;
using GrKouk.Nop.Api.Models;

namespace GrKouk.Nop.Api.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductAttributeValue, ProductAttributeValueDto>();
        }
    }
}
