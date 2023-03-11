using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace SalesWebProject.ViewModels.DeparmentViewModel
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UpperName { get { return this.Name.ToUpper(); } }
    }
}
