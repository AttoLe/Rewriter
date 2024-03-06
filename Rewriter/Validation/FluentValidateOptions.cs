using FluentValidation;
using Microsoft.Extensions.Options;
using Rewriter.Extensions;

namespace Rewriter.Validation;

public class FluentValidateOptions<TOptions>(ILogger? logger, IServiceProvider serviceProvider, string? name)
    : IValidateOptions<TOptions> where TOptions : class
{
    public ValidateOptionsResult Validate(string? inputName, TOptions options)
    {
        if (name is not null && name != inputName)
        {
            return ValidateOptionsResult.Skip;
        }

        if (options is null)
        {
            logger?.LogNoOptionsError(typeof(TOptions).Name);
            Environment.Exit(1);
        }

        using var scope = serviceProvider.CreateScope();
        var validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();
        var result = validator.Validate(options);

        if (result.IsValid)
        {
            logger?.LogSuccessOptionValidation(typeof(TOptions).Name);
            return ValidateOptionsResult.Success;
        }

        var type = options.GetType().Name;
        result.Errors.ForEach(item =>
            logger?.LogErrorOptionValidation(type, item.PropertyName, item.ErrorMessage));
        
        Environment.Exit(1);
        return ValidateOptionsResult.Fail(
            result.Errors.Select(x => x.PropertyName + "\t" + x.ErrorMessage));
    }
}