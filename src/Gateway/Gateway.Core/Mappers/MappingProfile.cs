using AutoMapper;
using Gateway.Acquiring.Contracts.Models;
using Gateway.Contracts.Models;
using Gateway.Data.Contracts.Entities;

namespace Gateway.Core.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PaymentRequestModel, PaymentDetails>()
                .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount))
                .ForMember(d => d.Currency, o => o.MapFrom(s => s.Currency))
                .ForMember(d => d.Cvv, o => o.MapFrom(s => s.Card.Cvv))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Card.Name))
                .ForMember(d => d.ExpiryMonth, o => o.MapFrom(s => s.Card.ExpiryMonth))
                .ForMember(d => d.ExpiryYear, o => o.MapFrom(s => s.Card.ExpiryYear))
                .ForMember(d => d.Number, o => o.MapFrom(s => s.Card.Number));

            CreateMap<MerchantAcquirerEntity, AcquirerDetails>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.MerchantName))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.Url));

            CreateMap<ProcessorResponse, PaymentResponseModel>()
                .ForPath(d => d.Card.Name, o => o.MapFrom(s => s.Name))
                .ForPath(d => d.Card.ExpiryMonth, o => o.MapFrom(s => s.ExpiryMonth))
                .ForPath(d => d.Card.ExpiryYear, o => o.MapFrom(s => s.ExpiryYear))
                .ForPath(d => d.Card.Number, o => o.MapFrom(s => s.Number));
        }
    }
}