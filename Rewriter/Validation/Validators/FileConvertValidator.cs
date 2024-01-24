using FluentValidation;
using Rewriter.Configuration;

namespace Rewriter.Validation;

public class FileConvertValidator : AbstractValidator<FileConvertConfig>
{
    public FileConvertValidator()
    {
        RuleFor(x => x.OutputFolderPath).SetValidator(new PathValidator());
        RuleForEach(x => x.InputFolderPathes)
            .NotEmpty().WithMessage("should be at least one output path")
            .SetValidator(new PathValidator());
    }
}