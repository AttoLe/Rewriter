using FluentValidation;
using Microsoft.Extensions.Options;

namespace Rewriter.Validation;

public class FluentValidateOptions<TOptions>(IServiceProvider serviceProvider, string? name) 
    : IValidateOptions<TOptions> where TOptions: class
{
    public ValidateOptionsResult Validate(string? inputName, TOptions options)
    {
        if (name is not null && name != inputName)
        {
            return ValidateOptionsResult.Skip;
        }

        if (options is null)
        {
            throw new ArgumentNullException(nameof(options), "is null");
        }

        using var scope = serviceProvider.CreateScope();
        var validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();
        var result = validator.Validate(options);

        if (result.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        var type = options.GetType().Name;
        var errors = result.Errors.Select(item =>
            $"Validation failed for {type}.{item.PropertyName} with the error: {item.ErrorMessage}");
        
        return ValidateOptionsResult.Fail(errors);
    }
}