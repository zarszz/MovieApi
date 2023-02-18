using System;
using FluentValidation;
using MovieAPi.DTOs.V1.Request;

namespace MovieAPi.Validator
{
    public class CreateMovieScheduleValidator : AbstractValidator<CreateMovieScheduleDto>
    {
        public CreateMovieScheduleValidator()
        {
            RuleFor(dto => dto.MovieId).NotEmpty();
            RuleFor(dto => dto.StudioId).NotEmpty();
            RuleFor(dto => dto.StartTime).NotEmpty().Must(d => d.Subtract(DateTime.Now).Days > 5)
                .WithMessage("Start time must be at least 5 days from now");
            RuleFor(dto => dto.EndTime).NotEmpty().Must(d => d.Subtract(DateTime.Now).Days > 5)
                .WithMessage("End time must be at least 5 days from now");
            RuleFor(dto => dto.Price).NotEmpty().Must(p => p > 0)
                .WithMessage("Price must be greater than 0");
            RuleFor(dto => dto.Date).NotEmpty().Must(d => d.Subtract(DateTime.Now).Days > 5)
                .WithMessage("Date must be at least 5 days from now");
        }
    }
}