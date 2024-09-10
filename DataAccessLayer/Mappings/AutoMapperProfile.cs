using AutoMapper;
using Common.Shared.DTOs;
using DataAccessLayer.Models;

namespace DataAccessLayer.Mappings
{
    // Configures the AutoMapper profiles for mapping between entities and DTOs.
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Supplier, SupplierDTO>().ReverseMap();
            CreateMap<Supplier, SupplierCreateDTO>().ReverseMap();
        }
    }
}
