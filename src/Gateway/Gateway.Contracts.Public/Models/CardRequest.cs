﻿namespace Gateway.Contracts.Public.Models
{
    public class CardRequest
    {
        public string Number { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Cvv { get; set; }
        public string Name { get; set; }
    }
}