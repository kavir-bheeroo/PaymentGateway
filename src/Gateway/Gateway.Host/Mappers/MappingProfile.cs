using AutoMapper;
using Gateway.Contracts.Models;
using Gateway.Contracts.Public.Models;
using System.Security.Claims;
using Gateway.Common.Extensions;

namespace Gateway.Host.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PaymentRequest, PaymentRequestModel>();
            CreateMap<ClaimsPrincipal, MerchantModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.GetMerchantId()))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.GetMerchantName()))
                .ForMember(d => d.SecretKey, o => o.MapFrom(s => s.GetMerchantSecretKey()));
        }
    }
}