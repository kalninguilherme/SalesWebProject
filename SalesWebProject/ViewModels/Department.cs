using Microsoft.Build.Framework;
using SalesWebProject.Enums;
using SalesWebProject.Helpers;
using System.ComponentModel.DataAnnotations;

namespace SalesWebProject.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<SellerViewModel> Sellers { get; set; }
    }
}
