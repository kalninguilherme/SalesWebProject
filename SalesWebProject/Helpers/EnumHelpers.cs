using Microsoft.AspNetCore.Mvc.Rendering;
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

        public static List<SelectListItem> GenerateSelectList<T>() where T : Enum
        {
            var query = (from m in Enum.GetValues(typeof(T)).Cast<T>().ToList()
                         select new SelectListItem()
                         {
                             Text = m.GetDescription(),
                             Value = m.ToString()
                         }).ToList();
            return query;
        }
    }
}

