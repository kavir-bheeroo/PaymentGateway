using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Data.Contracts.Entities
{
    public class MerchantAcquirerEntity
    {
        public int Id { get; set; }
        public int MerchantId { get; set; }
        public int AcquirerId { get; set; }

        public string MerchantName { get; set; }
        public string SecretKey { get; set; }

        public string AcquirerName { get; set; }
        public string Url { get; set; }
    }
}