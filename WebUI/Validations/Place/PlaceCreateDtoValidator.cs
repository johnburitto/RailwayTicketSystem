using Core.Dtos.Create;
using FluentValidation;

namespace WebUI.Validations.Place
{
    public class PlaceCreateDtoValidator : AbstractValidator<PlaceCreateDto>
    {
        public PlaceCreateDtoValidator()
        {
            RuleFor(place => place.Number)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Номер місця не може бути від'ємним або дорівнювати 0");
            RuleFor(place => place.Price)
                .NotNull()
                .GreaterThanOrEqualTo(0)
                .WithMessage("Цінна не може бути менша за 0");
            RuleFor(place => place.PlaceType)
                .NotNull()
                .IsInEnum();
            RuleFor(place => place.IsAvaliable)
                .NotNull();
            RuleFor(trainCar => trainCar.TrainCarId)
                .NotNull()
                .WithMessage("Id вагона необхідний");
        }
    }
}
