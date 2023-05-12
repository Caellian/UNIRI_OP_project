using System;

namespace Wordel.Util;

public class Ranged<T> where T: IComparable<T>
{
    public T Min;
    public bool StartInclusive = true;
    
    public T Max;
    public bool EndInclusive = false;

    public Ranged(T min, T max)
    {
        Min = min;
        Max = max;
    }

    public bool Includes(T value) =>
        ((Min.CompareTo(value) <= 0 && StartInclusive) || Min.CompareTo(value) < 0) &&
        ((value.CompareTo(Max) <= 0 && EndInclusive  ) || value.CompareTo(Max) < 0);
}