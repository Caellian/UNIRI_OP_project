using System;

namespace Wordel.Util;

[AttributeUsage(AttributeTargets.Field)]
public class Configurable: Attribute
{
    public string Name;
    
    public object? Default;

    /// <summary>
    /// Min and max values of configurable field.
    /// </summary>
    public Ranged<IComparable<object>>? Limits;
    
    public Configurable(string name, object? defaultValue = default, Ranged<IComparable<object>>? limits = null)
    {
        Name = name;
        Default = defaultValue;
        Limits = limits;
    }
}