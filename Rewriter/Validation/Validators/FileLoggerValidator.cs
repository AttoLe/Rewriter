using FluentValidation;
using Rewriter.Configuration;

namespace Rewriter.Validation.Validators;

public class FileLoggerValidator : AbstractValidator<FileLoggerOptions>
{
    public FileLoggerValidator()
    {
        RuleFor(loggerConfig => loggerConfig.FolderPath).SetValidator(new PathValidator());
    }
}