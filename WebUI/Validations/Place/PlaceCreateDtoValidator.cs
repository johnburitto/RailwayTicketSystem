using Core.Dtos.Create;
using FluentValidation;

namespace WebUI.Validations.Place
{
    public class PlaceCreateDtoValidator : AbstractValidator<PlaceCreateDto>
    {
        public PlaceCreateDtoValidator()
        {
            RuleFor(place => place.Price)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .WithMessage("Цінна не може бути менша за 0");
            RuleFor(place => place.PlaceType)
                .NotNull()
                .NotEmpty()
                .IsInEnum();
            RuleFor(place => place.IsAvaliable)
                .NotNull()
                .NotEmpty();
            RuleFor(trainCar => trainCar.TrainCarId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id вагона необхідний");
        }
    }
}
