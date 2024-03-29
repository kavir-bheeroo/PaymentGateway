﻿using AutoMapper;
using Gateway.Common.Extensions;
using Gateway.Contracts.Models;
using Gateway.Contracts.Public.Models;
using System.Security.Claims;

namespace Gateway.Host.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CardRequest, CardRequestModel>();
            CreateMap<CardResponseModel, CardResponse>();

            CreateMap<PaymentRequest, PaymentRequestModel>();

            CreateMap<PaymentResponseModel, PaymentResponse>();

            CreateMap<ClaimsPrincipal, MerchantModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.GetMerchantId()))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.GetMerchantName()))
                .ForMember(d => d.SecretKey, o => o.MapFrom(s => s.GetMerchantSecretKey()));
        }
    }
}