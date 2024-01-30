using System.Text.RegularExpressions;
using FluentValidation;

namespace Rewriter.Validation.Validators;

public class PathValidator : AbstractValidator<string>
{
    public PathValidator()
    {
        RuleFor(path => path).Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Must(path => Regex.IsMatch(path, @"^[A-Za-z]\:(\/[a-zA-Z0-9_\-\.\s]+)*$"))
            .WithMessage("path is written in incorrect form");
    }
}