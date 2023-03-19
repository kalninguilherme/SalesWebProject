using SalesWebProject.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SalesWebProject.ViewModels
{
    public class DepartmentsViewModel
    {
        public int Id { get; set; }


        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é necessário")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres")]
        public string Name { get; set; }

        public List<SellersViewModel> Sellers { get; set; } = new List<SellersViewModel>();
    }
}
