using FluentValidation;
using SocialMediaAPI.Common.DTO;

namespace SocialMediaAPI.Application.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(dto => dto.Login)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(dto => dto.Password)
                .NotEmpty()
                .Length(8, 20);
        }
    }
}
