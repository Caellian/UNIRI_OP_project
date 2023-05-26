using System;
using System.Linq;

namespace Wordel.Util;

public enum ConfigKind
{
    Integer,
    Float,
    String,
    Option,
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class Configurable: Attribute
{
    public string Name;
    public ConfigKind Kind;
    public object? Default;

    public object[] Choices;
    
    /// <summary>
    /// Min and max values of configurable field.
    /// </summary>
    public (object, object)? Limits;

    public decimal? Increment;
    
    public Configurable(string name, object[] choices, uint defaultValue = 0)
    {
        Name = name;
        Default = choices.GetValue(defaultValue);
        Kind = ConfigKind.Option;
        Choices = choices;
    }
    
    public Configurable(string name, ConfigKind kind, object? defaultValue = default)
    {
        Name = name;
        Default = defaultValue;
        Kind = kind;
    }
    
    public Configurable(string name, int defaultValue, int min, int max, float increment = 1)
    {
        Name = name;
        Default = defaultValue;
        Limits = (min, max);
        Kind = ConfigKind.Integer;
        Increment = new decimal(increment);
    }
    
    public Configurable(string name, float defaultValue, float min, float max, float increment = 1)
    {
        Name = name;
        Default = defaultValue;
        Limits = (new decimal(min), new decimal(max));
        Kind = ConfigKind.Float;
        Increment = new decimal(increment);
    }
}