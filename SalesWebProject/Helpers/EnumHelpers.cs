using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel;
using System.Reflection;

namespace SalesWebProject.Helpers
{
    public static class EnumHelpers
    {
        public static string GetDescription(this Enum enumerator)
        {
            var item = enumerator.GetType().GetField(enumerator.ToString());
            DescriptionAttribute field = item.GetCustomAttribute<DescriptionAttribute>();
            if (field == null)
            {
                return enumerator.ToString();
            }
            string description = field.Description.ToString();
            return description;
        }
    }
}
