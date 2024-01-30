using FluentValidation;
using Rewriter.Configuration;

namespace Rewriter.Validation.Validators;

public class FileOutputValidator : AbstractValidator<FileOutputOptions>
{
    public FileOutputValidator()
    {
        RuleFor(fileOutputOptions => fileOutputOptions.FolderPath).SetValidator(new PathValidator());
    }
}