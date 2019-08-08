using CreditCardValidator;
using FluentValidation;
using Gateway.Contracts.Public.Models;
using System;

namespace Gateway.Host.Validators
{
    public class CardRequestValidator : AbstractValidator<CardRequest>
    {
        public CardRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Number)
                .Must(number => Luhn.CheckLuhn(number));

            RuleFor(x => x.ExpiryMonth)
                .InclusiveBetween(1, 12);

            RuleFor(x => x.ExpiryYear)
                .InclusiveBetween(DateTime.UtcNow.AddYears(-1).Year, DateTime.UtcNow.AddYears(10).Year);

            RuleFor(x => x.Cvv)
                .NotEmpty();
        }
    }
}