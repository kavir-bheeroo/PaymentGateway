using CreditCardValidator;
using FluentValidation;
using Gateway.Contracts.Models;
using System;

namespace Gateway.Core.Validators
{
    public class CardRequestModelValidator : AbstractValidator<CardRequestModel>
    {
        public CardRequestModelValidator()
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