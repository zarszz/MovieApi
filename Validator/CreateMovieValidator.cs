using FluentValidation;
using MovieAPi.DTOs.V1.Request;

namespace MovieAPi.Validator
{
    public class CreateMovieValidator : AbstractValidator<CreateMovieDto>
    {
        public CreateMovieValidator()
        {
            RuleFor(dto => dto.Title).NotEmpty();
            RuleFor(dto => dto.Overview).NotEmpty();
            RuleFor(dto => dto.Poster).NotEmpty();
            RuleFor(dto => dto.PlayUntil).NotEmpty();
            RuleFor(dto => dto.Tags).Must(d => d.Length > 0);
        }
    }
}