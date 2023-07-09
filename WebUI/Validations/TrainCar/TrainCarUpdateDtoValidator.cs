using Core.Dtos.Update;
using FluentValidation;

namespace WebUI.Validations.TrainCar
{
    public class TrainCarUpdateDtoValidator : AbstractValidator<TrainCarUpdateDto>
    {
        public TrainCarUpdateDtoValidator()
        {
            RuleFor(trainCar => trainCar.Number)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .WithMessage("Номер вагона повинен містити не менше 1 символу")
                .MaximumLength(10)
                .WithMessage("Номер вагона повинен містити не більше 10 символів");
            RuleFor(place => place.TrainCarType)
                .NotNull()
                .IsInEnum();
            RuleFor(trainCar => trainCar.TrainId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id потяга необхідний");
        }
    }
}
