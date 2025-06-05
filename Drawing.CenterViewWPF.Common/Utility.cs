using System;
using System.ComponentModel;

namespace Drawing.CenterViewWPF.Common;

public class Utility
{
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