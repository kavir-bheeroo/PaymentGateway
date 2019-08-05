using Dapper;
using Gateway.Common;
using Gateway.Common.Exceptions;
using Gateway.Data.Contracts.Entities;
using Gateway.Data.Contracts.Interfaces;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Gateway.Data.Dapper.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DatabaseOptions _databaseOptions;

        public PaymentRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = Guard.IsNotNull(databaseOptions, nameof(databaseOptions)).Value;
        }

        public async Task<PaymentEntity> GetByIdAsync(Guid id)
        {
            Guard.IsNotNull(id, nameof(id));

            const string query = @"
                SELECT	id,
                        merchant_id,
                        merchant_name,
                        acquirer_id,
                        acquirer_name,
                        status,
                        response_code,
                        amount,
                        currency,
                        card_number,
                        expiry_month,
                        expiry_year,
                        cardholder_name,
                        acquirer_payment_id,
                        acquirer_status,
                        acquirer_response_code,
                        payment_time
                FROM	payments
                WHERE	id = @Id
            ";

            var parameters = new { Id = id.ToString() };

            using (var connection = new NpgsqlConnection(_databaseOptions.GatewayDatabaseConnectionString))
            {
                var entity = await connection.QueryFirstOrDefaultAsync<PaymentEntity>(query, parameters);
                return entity;
            }
        }

        public async Task<PaymentEntity> InsertAsync(PaymentEntity payment)
        {
            Guard.IsNotNull(payment, nameof(payment));

            const string query = @"
                INSERT INTO payments
                (
                        id,
                        merchant_id,
                        merchant_name,
                        acquirer_id,
                        acquirer_name,
                        status,
                        response_code,
                        amount,
                        currency,
                        card_number,
                        expiry_month,
                        expiry_year,
                        cardholder_name,
                        acquirer_payment_id,
                        acquirer_status,
                        acquirer_response_code,
                        payment_time
                )
                VALUES
                (
                        @Id,
                        @MerchantId,
                        @MerchantName,
                        @AcquirerId,
                        @AcquirerName,
                        @Status,
                        @ResponseCode,
                        @Amount,
                        @Currency,
                        @CardNumber,
                        @ExpiryMonth,
                        @ExpiryYear,
                        @CardholderName,
                        @AcquirerPaymentId,
                        @AcquirerStatus,
                        @AcquirerResponseCode,
                        @PaymentTime
                )
            ";

            var parameters = new
            {
                Id = payment.PaymentId,
                payment.MerchantId,
                payment.MerchantName,
                payment.AcquirerId,
                payment.AcquirerName,
                payment.Status,
                payment.ResponseCode,
                payment.Amount,
                payment.Currency,
                payment.CardNumber,
                payment.ExpiryMonth,
                payment.ExpiryYear,
                payment.CardholderName,
                payment.AcquirerPaymentId,
                payment.AcquirerStatus,
                payment.AcquirerResponseCode,
                payment.PaymentTime
            };

            using (var connection = new NpgsqlConnection(_databaseOptions.GatewayDatabaseConnectionString))
            {
                var affectedRows = await connection.ExecuteAsync(query, parameters);

                if (affectedRows < 1)
                {
                    throw new GatewayException("Insert payment to data store failed.");
                }

                return payment;
            }
        }

        public async Task UpdateAsync(PaymentEntity payment)
        {
            Guard.IsNotNull(payment, nameof(payment));

            const string query = @"
                UPDATE payments
                SET
                        status = @Status,
                        response_code = @ResponseCode,
                        acquirer_payment_id = @AcquirerPaymentId,
                        acquirer_status = @AcquirerStatus,
                        acquirer_response_code = @AcquirerResponseCode
                WHERE   id = @Id
            ";

            var parameters = new
            {
                Id = payment.PaymentId.ToString(),
                payment.Status,
                payment.ResponseCode,
                payment.AcquirerPaymentId,
                payment.AcquirerStatus,
                payment.AcquirerResponseCode
            };

            using (var connection = new NpgsqlConnection(_databaseOptions.GatewayDatabaseConnectionString))
            {
                var affectedRows = await connection.ExecuteAsync(query, parameters);

                if (affectedRows < 1)
                {
                    throw new GatewayException("Update payment failed.");
                }
            }
        }
    }
}