using SalesWebProject.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SalesWebProject.ViewModels
{
    public class SellerMainViewModel
    {
        public string Name { get; set; }

        public int Counter { get; set; }

        public List<SellerViewModel> Sellers { get; set; }
    }

    public class SellerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo {0} necessário")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo {0} necessário")]
        [EmailAddress(ErrorMessage = "Entre com um e-mail válido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo {0} necessário")]
        [Display(Name = "Data de Nascimento")]
        public DateTime BirthDate { get; set; }
        public string BirthDateString { get { return this.BirthDate.ToShortDateString(); } }


        [Required(ErrorMessage = "Campo {0} necessário")]
        [Display(Name = "Salário Base")]
        public double BaseSalary { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public List<SalesRecordViewModel> Sales { get; set; } = new List<SalesRecordViewModel>();

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales.Where(m => m.Date >= initial && m.Date <= final).Sum(m => m.Amount);
        }

    }
}
