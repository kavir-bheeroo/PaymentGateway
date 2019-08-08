using FluentValidation;
using Gateway.Contracts.Public.Models;

namespace Gateway.Host.Validators
{
    public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidator()
        {
            RuleFor(x => x.TrackId)
                .NotEmpty();

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3);

            RuleFor(x => x.Card)
                .InjectValidator();
        }
    }
}