using FluentValidation;
using SocialMediaAPI.Common.DTO;

namespace SocialMediaAPI.Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(dto => dto.BirthDate)
                .NotEmpty()
                .Must(date => date.AddYears(12) <= DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("You cannot register here if you are under 12 y.o.");

            RuleFor(dto => dto.CountryId)
                .GreaterThan(0)
                .WithMessage("Country must be specified.");

            RuleFor(dto => dto.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256);

            RuleFor(dto => dto.Gender)
                .NotNull()
                .IsInEnum();

            RuleFor(dto => dto.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(dto => dto.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(dto => dto.Password)
                .NotEmpty()
                .Matches(@"[A-z0-9@#$%^&?!:;*()/\\]")
                .Length(8, 20);

            RuleFor(dto => dto.UserName)
                .Length(3, 50);
        }
    }
}
