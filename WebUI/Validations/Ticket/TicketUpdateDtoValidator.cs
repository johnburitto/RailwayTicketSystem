using Core.Dtos.Update;
using FluentValidation;

namespace WebUI.Validations.Ticket
{
    public class TicketUpdateDtoValidator : AbstractValidator<TicketUpdateDto>
    {
        public TicketUpdateDtoValidator()
        {
            RuleFor(ticket => ticket.BookDate)
                .Must(date => !date.Equals(default))
                .WithMessage("Дата та час броювання необхідні");
            RuleFor(ticket => ticket.PlaceId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id місця необхідний");
        }
    }
}
