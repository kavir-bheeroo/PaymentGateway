using AutoMapper;
using Gateway.Acquiring.BankSimulator.Models;
using Gateway.Acquiring.Contracts.Models;

namespace Gateway.Acquiring.BankSimulator.Mappers
{
    public class BankSimulatorMapper : Profile
    {
        public BankSimulatorMapper()
        {
            CreateMap<ProcessorRequest, BankSimulatorRequest>()
                .ForMember(d => d.GatewayPaymentId, o => o.MapFrom(s => s.PaymentId))
                .ForMember(d => d.Amount, o => o.MapFrom(s => s.PaymentDetails.Amount))
                .ForMember(d => d.Currency, o => o.MapFrom(s => s.PaymentDetails.Currency))
                .ForMember(d => d.CardNumber, o => o.MapFrom(s => s.PaymentDetails.Number))
                .ForMember(d => d.Cvv, o => o.MapFrom(s => s.PaymentDetails.Cvv))
                .ForMember(d => d.ExpiryMonth, o => o.MapFrom(s => s.PaymentDetails.ExpiryMonth))
                .ForMember(d => d.ExpiryYear, o => o.MapFrom(s => s.PaymentDetails.ExpiryYear))
                .ForMember(d => d.CardholderName, o => o.MapFrom(s => s.PaymentDetails.Name));

            CreateMap<BankSimulatorResponse, ProcessorResponse>()
                .ForMember(d => d.AcquirerPaymentId, o => o.MapFrom(s => s.PaymentId))
                .ForMember(d => d.PaymentId, o => o.MapFrom(s => s.GatewayPaymentId));
        }
    }
}