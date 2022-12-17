using FluentValidation;

namespace API.Model
{
    public record InputModel(string FullName, string Alignment, string GroupAffiliation);

    public class InputModelValidator : AbstractValidator<InputModel>
    {
        public InputModelValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MinimumLength(12);

            RuleFor(x => x.Alignment)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(x => x.GroupAffiliation)
                .NotEmpty()
                .MinimumLength(5);
        }
    }
}
