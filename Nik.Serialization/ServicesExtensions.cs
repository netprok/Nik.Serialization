﻿namespace Nik.Serialization;

public static class ServicesExtensions
{
    public static IServiceCollection UseSerialization(this IServiceCollection services)
    {
        services.AddSingleton<ICsvSerializer, CsvSerializer>();
        services.AddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();
        services.AddSingleton<IXMLSerializer, XMLSerializer>();

        return services;
    }
}