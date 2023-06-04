using Core.Dtos.Create;
using FluentValidation;

namespace WebUI.Validations.TrainCar
{
    public class TrainCarCreateDtoValidator : AbstractValidator<TrainCarCreateDto>
    {
        public TrainCarCreateDtoValidator()
        {
            RuleFor(trainCar => trainCar.Number)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .WithMessage("Номер вагона повинен містити не менше 1 символу")
                .MaximumLength(10)
                .WithMessage("Номер вагона повинен містити не більше 10 символів");
            RuleFor(trainCar => trainCar.TrainId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id потяга необхідний");
        }
    }
}
