using Microsoft.Build.Framework;
using SalesWebProject.Enums;
using SalesWebProject.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SalesWebProject.ViewModels
{
    public class DepartmentsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        public List<SellersViewModel> Sellers { get; set; } = new List<SellersViewModel>();
    }
}
