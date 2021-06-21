using GitHookProcessor.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GitHookProcessor.Common.Tools
{
    public class EnumTools
    {
        public static Dictionary<string, T> GetDescriptionsDict<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .OfType<T>()
                .ToDictionary(value => value.GetDescription(), value => value);
        }
    }
}