using FluentValidation;
using MovieAPi.DTOs.V1.Request;

namespace MovieAPi.Validator
{
    public class CreateStudioValidator : AbstractValidator<CreateStudioDto>
    {
        public CreateStudioValidator()
        {
            RuleFor(dto => dto.StudioNumber).NotEmpty();
            RuleFor(dto => dto.SeatCapacity).NotEmpty();
        }
    }
}