using FluentValidation;
using MovieAPi.DTOs.V1.Request;

namespace MovieAPi.Validator
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(c => c.PaymentMethod).NotEmpty();
            RuleFor(c => c.OrderItems).Must(d => d.Count > 0);
        }
    }
}