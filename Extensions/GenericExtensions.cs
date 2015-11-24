using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class GenericExtensions
{
    public static bool IsDefault<T>(this T value) where T : struct
    {
        bool isDefault = value.Equals(default(T));

        return isDefault;
    }
}
