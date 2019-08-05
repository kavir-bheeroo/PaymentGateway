using AutoMapper;
using Gateway.Contracts.Models;
using Gateway.Contracts.Public.Models;
using System.Security.Claims;
using Gateway.Common.Extensions;
using Gateway.Common;

namespace Gateway.Host.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CardRequest, CardRequestModel>();
            CreateMap<CardResponseModel, CardResponse>();

            CreateMap<PaymentRequest, PaymentRequestModel>();

            CreateMap<PaymentResponseModel, PaymentResponse>()
                .ForMember(d => d.Status, o => o.MapFrom(s => s.ResponseCode.Equals(Constants.SuccessfulResponseCode) ? "Payment Successful" : "Payment Failed"));

            CreateMap<ClaimsPrincipal, MerchantModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.GetMerchantId()))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.GetMerchantName()))
                .ForMember(d => d.SecretKey, o => o.MapFrom(s => s.GetMerchantSecretKey()));
        }
    }
}