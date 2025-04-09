using AutoMapper;
using Int.Domain.DTOs.Cars;
using Int.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Int.Domain.Mapping.Cars
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<Car, CarDTO>()
                .ForMember(cd => cd.imageUrls, c => c.MapFrom(c => c.CarPhotos.Select(cr => cr.imageUrl)));

            //CreateMap<Brand, BrandDTO>()
            //    .ForMember(d => d.BrandCode, option => option.MapFrom(s => s.BCode))
            //    .ForMember(d => d.BrandName, option => option.MapFrom(s => s.BName));


            //CreateMap<Color, ColorDTO>()
            //    .ForMember(d => d.ColorCode, option => option.MapFrom(s => s.CCode))
            //    .ForMember(d => d.ColorName, option => option.MapFrom(s => s.CName));


            CreateMap<UploadCarDTO, Car>();


        }
    }
}
