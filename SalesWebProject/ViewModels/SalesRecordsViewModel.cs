using SalesWebProject.Enums;
using SalesWebProject.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesWebProject.ViewModels
{
    public class SalesRecordsMainViewModel
    {
        public SalesRecordsMainViewModel()
        {
            Filter = new SalesRecordFilter();
        }

        public List<SalesRecordsGroupedViewModel> SalesRecordsGroups { get; set; }
        public SalesRecordFilter Filter { get; set; }
    }

    public class SalesRecordsGroupedViewModel
    {
        public string Name { get; set; }

        public int Counter { get; set; }

        public List<SalesRecordsViewModel> SalesRecords { get; set; }
    }

    public class SalesRecordsViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo {0} requerido")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string DateString { get { return Date.ToShortDateString(); } }

        [Required(ErrorMessage = "Campo {0} requerido")]
        public double Amount { get; set; }

        public SaleStatusEnum Status { get; set; }

        public string DepartmentName { get; set; }

        [Display(Name = "Vendedor")]
        [Required(ErrorMessage = "Campo de vendedor requerido")]
        public int SellerId { get; set; }

        public string SellerName { get; set; }

        public Seller Seller { get; set; }
    }

    public class SalesRecordFilter
    {
        public SalesRecordFilter()
        {
            DateEnd = DateTime.Now;
            DateStart = DateTime.Now.AddYears(-5);
            Status = SalesRecordsGroupType.Department;
        }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateTime? DateStart { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]

        public DateTime? DateEnd { get; set; }

        public SalesRecordsGroupType Status { get; set; }

        public bool AllPeriods { get; set; }
    }

    public enum SalesRecordsGroupType
    {
        [Description("Departamento")]
        Department = 1,

        [Description("Status da Venda")]
        Status = 2,

        [Description("Vendedor")]
        Seller = 3
    }
}
