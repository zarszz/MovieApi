using FluentValidation;
using MovieAPi.DTOs.V1.Request;

namespace MovieAPi.Validator
{
    public class CreateTagValidator : AbstractValidator<CreateTagDto>
    {
        public CreateTagValidator()
        {
            RuleFor(t => t.Name).NotEmpty().Must(n => n.Length > 3)
                .WithMessage("Name must be at least 3 characters long");
        }
    }
}