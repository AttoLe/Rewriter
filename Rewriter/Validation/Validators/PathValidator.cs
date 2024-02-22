using FluentValidation;

namespace Rewriter.Validation.Validators;

public class PathValidator : AbstractValidator<string>
{
    public PathValidator()
    {
        RuleFor(path => path).Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Must(Directory.Exists)
            .WithMessage("Required path do not exists");
    }
}