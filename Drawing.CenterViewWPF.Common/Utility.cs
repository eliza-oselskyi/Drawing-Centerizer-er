using System;
using System.ComponentModel;

namespace Drawing.CenterViewWPF.Common;

/// <summary>
/// Provides utility methods for common operations that involve enums or other reusable functionality.
/// </summary>
public class Utility
{
    /// <summary>
    /// Retrieves an enum value of type <typeparamref name="T"/> based on its <see cref="DescriptionAttribute"/> or its name.
    /// </summary>
    /// <typeparam name="T">The enum type from which to retrieve the value.</typeparam>
    /// <param name="description">The description or name of the enum value to retrieve.</param>
    /// <returns>The enum value of type <typeparamref name="T"/> that matches the provided description or name.</returns>
    /// <exception cref="InvalidOperationException">Thrown if <typeparamref name="T"/> is not an enumeration type.</exception>
    /// <exception cref="ArgumentException">Thrown if no enum value matches the provided description or name.</exception>
    public static T GetValueFromDescription<T>(string description)
    {
        var type = typeof(T);
        if (!type.IsEnum)
            throw new InvalidOperationException();
        foreach (var field in type.GetFields())
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute
                customAttribute)
            {
                if (customAttribute.Description == description)
                    return (T)field.GetValue(null);
            }
            else if (field.Name == description)
            {
                return (T)field.GetValue(null);
            }

        throw new ArgumentException("Not found.", nameof(description));
    }   
}