using System.Text.RegularExpressions;
using FluentValidation;

namespace Rewriter.Validation;

public class PathValidator : AbstractValidator<string>
{
    public PathValidator()
    {
        RuleFor(path => path).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("path could not be null")
            .Must(path => Regex.IsMatch(@"^[A-Za-z]\:(\/[a-zA-Z0-9_\-\.\s]+)+$", path))
            .WithMessage("path is written in incorrect form");
    }
}