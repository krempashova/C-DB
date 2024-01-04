using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Diagnostics.Metrics;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSupplierDto, Supplier>();


            this.CreateMap<ImportPartDto, Part>()
                .ForMember(d => d.SupplierId,
                    opt => opt.MapFrom(s => s.SupplierId!.Value));

            this.CreateMap<ImportCarDto, Car>()
              .ForSourceMember(s => s.Parts, opt => opt.DoNotValidate());

            this.CreateMap<Car, ExportCarDto>();

            this.CreateMap<ImportCustomerDto, Customer>();


            this.CreateMap<ImportSaleDto, Sale>()
                .ForMember(d => d.CarId, 
                opt => opt.MapFrom(s => s.CarId!.Value));


            this.CreateMap<Car, ExportBmwCarDto>();

            this.CreateMap<Supplier,ExportSupplierDto>()
                .ForMember(x => x.PartsCount, y => y.MapFrom(x => x.Parts.Count()));

            this.CreateMap<Part, ExportCarPartDto>();
            this.CreateMap<Car, ExportCarWithPartsDto>()
              .ForMember(d => d.Parts,
                  opt => opt.MapFrom(s =>
                      s.PartsCars
                          .Select(pc => pc.Part)
                          .OrderByDescending(p => p.Price)
                          .ToArray()));
        }
    }
}
