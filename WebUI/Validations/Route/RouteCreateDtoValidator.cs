using Core.Dtos.Create;
using FluentValidation;

namespace WebUI.Validations.Route
{
    public class RouteCreateDtoValidator : AbstractValidator<RouteCreateDto>
    {
        public RouteCreateDtoValidator() 
        {
            RuleFor(route => route.DepartureTime)
                .Must(date => !date.Equals(default))
                .WithMessage("Дата та час відправлення необхідні");
            RuleFor(route => route.ArrivalTime)
                .Must(date => !date.Equals(default))
                .WithMessage("Дата та час прибуття необхідні");
            RuleFor(route => route.FromCity)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .WithMessage("Місто відправлення повинно містити не менше 3 символів")
                .MaximumLength(50)
                .WithMessage("Місто відправлення повинно містити не більше 50 символів");
            RuleFor(route => route.ToCity)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .WithMessage("Місто відправлення повинно містити не менше 3 символів")
                .MaximumLength(50)
                .WithMessage("Місто відправлення повинно містити не більше 50 символів");
        }
    }
}
