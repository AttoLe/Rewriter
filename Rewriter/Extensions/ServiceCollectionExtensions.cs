﻿namespace Rewriter.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFluentValidationOptions<TOptions>(this IServiceCollection serviceCollection,
        string sectionName) where TOptions : class
    {
        serviceCollection.AddOptions<TOptions>()
            .BindConfiguration(sectionName)
            .ValidateFluentValidation().ValidateOnStart();

        return serviceCollection;
    }
}