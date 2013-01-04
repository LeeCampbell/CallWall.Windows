using System;
using System.Linq;
using NUnit.Framework;

namespace CallWall.UnitTests
{
    public sealed class AllEnumValuesAttribute : ValuesAttribute
    {
        public AllEnumValuesAttribute(Type enumType)
        {
            if (enumType.IsEnum == false)
            {
                throw new InvalidOperationException(string.Format("{0} must be an enum type", enumType.Name));
            }
            data = Enum.GetValues(enumType).Cast<object>().ToArray();
        }
    }
}