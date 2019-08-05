using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Contracts.Public.Models
{
    public class CardResponse
    {
        public string Number { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Name { get; set; }
    }
}