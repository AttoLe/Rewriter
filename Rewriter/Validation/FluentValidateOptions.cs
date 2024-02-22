using FluentValidation;
using Microsoft.Extensions.Options;

namespace Rewriter.Validation;

public class FluentValidateOptions<TOptions>(IServiceProvider serviceProvider, string? name)
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
            throw new ArgumentNullException(nameof(options), "is null");
        }

        using var scope = serviceProvider.CreateScope();
        var validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();
        var result = validator.Validate(options);

        if (result.IsValid)
        {
            Console.WriteLine("Fluent validation for {0} success", typeof(TOptions));
            return ValidateOptionsResult.Success;
        }

        var type = options.GetType().Name;
        var errors = result.Errors.Select(item =>
            $"Fluent validation failed for {type}.{item.PropertyName} with the error: {item.ErrorMessage}").ToList();

        if (errors.Count == 0)
        {
            return ValidateOptionsResult.Fail(errors);
        }
        
        Console.WriteLine("Fluent validation for {0} failed", typeof(TOptions));
        result.Errors.ForEach(Console.WriteLine);
        
        return ValidateOptionsResult.Fail(errors);
    }
}