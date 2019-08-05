using Dapper.FluentMap.Mapping;
using Gateway.Data.Contracts.Entities;

namespace Gateway.Data.Dapper.Mappings
{
    public class PaymentEntityMap : EntityMap<PaymentEntity>
    {
        public PaymentEntityMap()
        {
            Map(m => m.PaymentId).ToColumn("id");
            Map(m => m.MerchantId).ToColumn("merchant_id");
            Map(m => m.MerchantName).ToColumn("merchant_name");
            Map(m => m.AcquirerId).ToColumn("acquirer_id");
            Map(m => m.AcquirerName).ToColumn("acquirer_name");
            Map(m => m.Status).ToColumn("status");
            Map(m => m.ResponseCode).ToColumn("response_code");
            Map(m => m.Amount).ToColumn("amount");
            Map(m => m.Currency).ToColumn("currency");
            Map(m => m.CardNumber).ToColumn("card_number");
            Map(m => m.ExpiryMonth).ToColumn("expiry_month");
            Map(m => m.ExpiryYear).ToColumn("expiry_year");
            Map(m => m.CardholderName).ToColumn("cardholder_name");
            Map(m => m.AcquirerPaymentId).ToColumn("acquirer_payment_id");
            Map(m => m.AcquirerStatus).ToColumn("acquirer_status");
            Map(m => m.AcquirerResponseCode).ToColumn("acquirer_response_code");
            Map(m => m.PaymentTime).ToColumn("payment_time");
        }
    }
}