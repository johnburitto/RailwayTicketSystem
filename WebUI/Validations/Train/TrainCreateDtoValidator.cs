using Core.Dtos.Create;
using FluentValidation;

namespace WebUI.Validations.Train
{
    public class TrainCreateDtoValidator : AbstractValidator<TrainCreateDto>
    {
        public TrainCreateDtoValidator()
        {
            RuleFor(train => train.Number)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .WithMessage("Номер потяга повинен містити не менше 3 символів")
                .MaximumLength(10)
                .WithMessage("Номер потяга повинен містити не більше 10 символів");
            RuleFor(train => train.RouteId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id маршруту необхідний");
        }
    }
}
