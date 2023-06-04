using Core.Dtos.Update;
using FluentValidation;

namespace WebUI.Validations.Train
{
    public class TrainUpdateDtoValidator : AbstractValidator<TrainUpdateDto>
    {
        public TrainUpdateDtoValidator()
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
