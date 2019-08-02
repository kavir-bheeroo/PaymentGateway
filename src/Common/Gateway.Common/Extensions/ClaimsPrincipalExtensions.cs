using System;
using System.Security.Claims;

namespace Gateway.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetMerchantId(this ClaimsPrincipal merchant)
        {
            var value = merchant?.FindFirst(ClaimTypes.MerchantId)?.Value;
            var isInt = int.TryParse(value, out var result);

            return string.IsNullOrEmpty(value) || !isInt ? (int?)null : result;
        }

        public static string GetMerchantName(this ClaimsPrincipal merchant)
        {
            var value = merchant?.FindFirst(ClaimTypes.MerchantName)?.Value;
            return value;
        }

        public static Guid? GetMerchantSecretKey(this ClaimsPrincipal merchant)
        {
            var value = merchant?.FindFirst(ClaimTypes.SecretKey)?.Value;
            var isGuid = Guid.TryParse(value, out var result);

            return string.IsNullOrEmpty(value) || !isGuid ? (Guid?)null : result;
        }
    }

    public static class ClaimTypes
    {
        public const string MerchantId = "merchantId";
        public const string MerchantName = "merchantName";
        public const string SecretKey = "secretKey";
    }
}