using FluentValidation;
using Rewriter.Configuration;

namespace Rewriter.Validation.Validators;

public class FileInputOptionsValidator : AbstractValidator<FileInputOptions>
{
    public FileInputOptionsValidator()
    {
        RuleFor(inputOptions => inputOptions.FileInputList)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Input options is required")
            .NotEmpty().WithMessage("At least one option is required")
            .ForEach(x =>
            {
                x.ChildRules(inputOption =>
                {
                    inputOption.RuleFor(option => option.FolderPaths)
                        .Cascade(CascadeMode.Stop)
                        .NotNull().WithMessage("Folder path is required")
                        .NotEmpty().WithMessage("Folder path is required")
                        .ForEach(path => path.SetValidator(new PathValidator()));

                    inputOption.RuleFor(option => option.Extensions)
                        .Cascade(CascadeMode.Stop)
                        .NotNull().WithMessage("Extensions list is required")
                        .NotEmpty().WithMessage("At least one extension must is required")
                        .ForEach(extensionRule =>
                        {
                            extensionRule.NotEmpty().WithMessage("Extension must not be empty");
                            extensionRule.Matches(@"^\.\w+$").WithMessage("Extension must start with a dot");
                        });
                });
            });
    }
}