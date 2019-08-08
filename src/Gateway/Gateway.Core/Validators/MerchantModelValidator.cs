using FluentValidation;
using Gateway.Contracts.Models;

namespace Gateway.Core.Validators
{
    public class MerchantModelValidator : AbstractValidator<MerchantModel>
    {
        public MerchantModelValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.SecretKey)
                .NotEmpty();
        }
    }
}