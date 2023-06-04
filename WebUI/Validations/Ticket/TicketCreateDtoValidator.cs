using Core.Dtos.Create;
using FluentValidation;

namespace WebUI.Validations.Ticket
{
    public class TicketCreateDtoValidator : AbstractValidator<TicketCreateDto>
    {
        public TicketCreateDtoValidator()
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
