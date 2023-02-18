using FluentValidation;
using MovieAPi.DTOs.V1.Request;

namespace MovieAPi.Validator
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator()
        {
            RuleFor(dto => dto.Email).NotEmpty();
            RuleFor(dto => dto.Password).NotEmpty();
            RuleFor(dto => dto.Avatar).NotEmpty();
            RuleFor(dto => dto.Name).NotEmpty();
        }
    }
}