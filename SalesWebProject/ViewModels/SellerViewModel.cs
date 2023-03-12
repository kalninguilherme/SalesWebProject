using SalesWebProject.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebProject.ViewModels
{
    public class SellerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo {0} requerido")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo {0} requerido")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo {0} requerido")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Campo {0} requerido")]
        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double BaseSalary { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public Department Department { get; set; }

        public List<SalesRecordViewModel> Sales { get; set; } = new List<SalesRecordViewModel>();

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales.Where(m => m.Date >= initial && m.Date <= final).Sum(m => m.Amount);
        }

    }
}
