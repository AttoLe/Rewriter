using FluentValidation;
using Rewriter.Configuration;

namespace Rewriter.Validation.Validators;

public class FileInputValidator : AbstractValidator<FileInputOptions>
{
    public FileInputValidator()
    {
        RuleFor(inputOptions => inputOptions.FolderPaths).Cascade(CascadeMode.Stop)
            .NotNull().NotEmpty()
            .WithMessage("should be at least one output path")
            .DependentRules(() =>
                RuleForEach(inputOptions => inputOptions.FolderPaths).Cascade(CascadeMode.Stop)
                    .NotNull().SetValidator(new PathValidator()));
        
        RuleFor(inputOptions => inputOptions.Extensions).NotNull().NotEmpty()
            .WithMessage("should be at least one input extension");
    }
}
