using FluentValidation;
using Rewriter.Configuration;

namespace Rewriter.Validation.Validators;

public class FileOutputOptionValidator : AbstractValidator<FileOutputOptions>
{
    public FileOutputOptionValidator()
    {
        RuleFor(fileOutputOptions => fileOutputOptions.FolderPath).SetValidator(new PathValidator());
    }
}