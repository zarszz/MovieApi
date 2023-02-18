using FluentValidation;
using MovieAPi.DTOs.V1.Request;

namespace MovieAPi.Validator
{
    public class LoginUserValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserValidator()
        {
            RuleFor(dto => dto.Email).NotEmpty();
            RuleFor(dto => dto.Password).NotEmpty();
        }
    }
}