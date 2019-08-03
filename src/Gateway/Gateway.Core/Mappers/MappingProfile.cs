using AutoMapper;
using Gateway.Acquiring.Contracts.Models;
using Gateway.Contracts.Models;

namespace Gateway.Core.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PaymentRequestModel, ProcessorRequest>()
                .ForMember(d => d.Cvv, o => o.MapFrom(s => s.Card.Cvv))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Card.Name))
                .ForMember(d => d.ExpiryMonth, o => o.MapFrom(s => s.Card.ExpiryMonth))
                .ForMember(d => d.ExpiryYear, o => o.MapFrom(s => s.Card.ExpiryYear))
                .ForMember(d => d.Number, o => o.MapFrom(s => s.Card.Number));

            CreateMap<ProcessorResponse, PaymentResponseModel>()
                .ForPath(d => d.Card.Name, o => o.MapFrom(s => s.Name))
                .ForPath(d => d.Card.ExpiryMonth, o => o.MapFrom(s => s.ExpiryMonth))
                .ForPath(d => d.Card.ExpiryYear, o => o.MapFrom(s => s.ExpiryYear))
                .ForPath(d => d.Card.Number, o => o.MapFrom(s => s.Number));
        }
    }
}