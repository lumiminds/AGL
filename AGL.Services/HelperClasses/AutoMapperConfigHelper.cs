using System;
using AGL.Models;
using AGL.Services.DTOs;
using AutoMapper;

namespace AGL.Services.HelperClasses
{
    public class AutoMapperConfigHelper : Profile
    {
        public AutoMapperConfigHelper()
        {
            CreateMap<People, PeopleDTO>().ReverseMap();
            CreateMap<Pet, PetDTO>().ReverseMap();
        }
    }
}
