using Microsoft.Extensions.Options;
using Rewriter.Validation;

namespace Rewriter.Extensions;

public static class OptionsBuilderExtension
{
    public static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(this OptionsBuilder<TOptions> builder)
        where TOptions : class
    {
        builder.Services.AddSingleton<IValidateOptions<TOptions>>(serviceProvider => 
            new FluentValidateOptions<TOptions>(serviceProvider, builder.Name));

        return builder;
    }
}