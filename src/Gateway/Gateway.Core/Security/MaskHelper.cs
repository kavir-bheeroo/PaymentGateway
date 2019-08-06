namespace Gateway.Core.Security
{
    public static class MaskHelper
    {
        public static string MaskCardNumber(string cardNumber)
        {
            var first6Digits = cardNumber.Substring(0, 6);
            var last4Digits = cardNumber.Substring(cardNumber.Length - 4, 4);

            return string.Concat(first6Digits, "****", last4Digits);
        }
    }
}