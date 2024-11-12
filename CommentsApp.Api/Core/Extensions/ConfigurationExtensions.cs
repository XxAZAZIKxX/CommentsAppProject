using System.Runtime.Serialization;
using System.Text.Json;
using CommentsApp.Api.Core.Exceptions;

namespace CommentsApp.Api.Core.Extensions;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Get section
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="SectionNotFoundException"></exception>
    public static IConfigurationSection GetSectionOrThrow(this IConfiguration configuration, string path)
    {
        var section = configuration.GetSection(path);
        if (section.Exists() is false) throw new SectionNotFoundException(section.Path);
        return section;
    }

    /// <summary>
    /// Get section value
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="SectionNotFoundException"></exception>
    /// <exception cref="SectionValueNotFoundException"></exception>
    public static string GetValueOrThrow(this IConfiguration configuration, string path)
    {
        var section = configuration.GetSectionOrThrow(path);
        if (section.Value is null) throw new SectionValueNotFoundException(section.Path);
        return section.Value;
    }

    /// <summary>
    /// Get section value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="configuration"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="SectionNotFoundException"></exception>
    /// <exception cref="SectionValueNotFoundException"></exception>
    public static T GetValueOrThrow<T>(this IConfiguration configuration, string path)
    {
        var section = configuration.GetSectionOrThrow(path);
        return section.Get<T>() ?? throw new SectionValueNotFoundException(path);
    }
}