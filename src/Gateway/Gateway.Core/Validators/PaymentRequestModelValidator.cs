using FluentValidation;
using Gateway.Common;
using Gateway.Contracts.Models;

namespace Gateway.Core.Validators
{
    public class PaymentRequestModelValidator : AbstractValidator<PaymentRequestModel>
    {
        private readonly IValidator<CardRequestModel> _cardRequestModelValidator;

        public PaymentRequestModelValidator(IValidator<CardRequestModel> cardRequestModelValidator)
        {
            _cardRequestModelValidator = Guard.IsNotNull(cardRequestModelValidator, nameof(cardRequestModelValidator));
        }

        public PaymentRequestModelValidator()
        {
            RuleFor(x => x.TrackId)
                .NotEmpty();

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3);

            RuleFor(x => x.Card)
                .SetValidator(_cardRequestModelValidator);
        }
    }
}