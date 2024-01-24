using FluentValidation;
using Rewriter.Configuration;

namespace Rewriter.Validation.Validators;

public class FileConvertValidator : AbstractValidator<FileConvertOptions>
{
    public FileConvertValidator()
    {
        RuleFor(convertConfig => convertConfig.OutputFolderPath).SetValidator(new PathValidator());
        RuleFor(convertConfig => convertConfig.InputFolderPaths).Cascade(CascadeMode.Stop)
            .NotNull().NotEmpty()
            .WithMessage("should be at least one output path")
            .DependentRules(() =>
                RuleForEach(convertConfig => convertConfig.InputFolderPaths).Cascade(CascadeMode.Stop)
                    .NotNull().SetValidator(new PathValidator()));
    }
}