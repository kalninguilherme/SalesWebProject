using SalesWebProject.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SalesWebProject.ViewModels
{
    public class SellersMainViewModel
    {
        public string Name { get; set; }

        public int Counter { get; set; }

        public double SumSalary { get; set; }

        public List<SellersViewModel> Sellers { get; set; }
    }

    public class SellersViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é necessário")]
        [Display(Name = "Nome")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "O {0} deve conter entre {2} e {1} caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é necessário")]
        [EmailAddress(ErrorMessage = "Entre com um e-mail válido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é necessário")]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:dd/MM/yyyy")]
        public DateTime BirthDate { get; set; }
        public string BirthDateString { get { return this.BirthDate.ToShortDateString(); } }

        [Required(ErrorMessage = "O campo {0} é necessário")]
        [Range(100.0, 50000.0, ErrorMessage = "{0} deve estar entre {1} e {2}")]
        [Display(Name = "Salário Base")]
        public double BaseSalary { get; set; }

        public string BaseSalaryMonetary { get { return this.BaseSalary.ToString("C", new CultureInfo("pt-Br")); } }

        public int DepartmentId { get; set; }

        public List<int> DepartmentIds { get; set; }

        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "Departamento Necessário")]
        public Department Department { get; set; }

        public List<SalesRecordsViewModel> Sales { get; set; } = new List<SalesRecordsViewModel>();

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales.Where(m => m.Date >= initial && m.Date <= final).Sum(m => m.Amount);
        }

    }
}
