using FluentValidation;
using Rewriter.Configuration;

namespace Rewriter.Validation.Validators;

public class FileLoggerOptionValidator : AbstractValidator<FileLoggerOptions>
{
    public FileLoggerOptionValidator()
    {
        RuleFor(loggerConfig => loggerConfig.FolderPath).SetValidator(new PathValidator());
    }
}