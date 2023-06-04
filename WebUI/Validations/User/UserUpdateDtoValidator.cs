using FluentValidation;
using Security.Dto;
using System.Text.RegularExpressions;

namespace WebUI.Validations.User
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(user => user.FirstName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2)
                .WithMessage("Ім'я повинне містити не менше 2 символів");
            RuleFor(user => user.MiddleName)
                .NotNull();
            RuleFor(user => user.LastName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2)
                .WithMessage("Прізвище повинно містити не менше 2 символів");
            RuleFor(user => user.UserName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .WithMessage("Логін повинен містити не менше 3 символів");
            RuleFor(user => user.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage("Пароль повиннен містити не менше 8 символів");
            RuleFor(user => user.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress();
            RuleFor(user => user.PhoneNumber)
                .NotNull()
                .NotEmpty()
                .MinimumLength(10)
                .WithMessage("Номер телефону повинен містити не менше 10 символів")
                .MaximumLength(20)
                .WithMessage("Номер телефону повинен містити не більше 20 символів")
                .Matches(new Regex(@"[+]\d{10,20}"))
                .WithMessage("Номер телефону некоректний");
            RuleFor(user => user.Role)
                .NotNull()
                .NotEmpty()
                .IsInEnum();
        }
    }
}
