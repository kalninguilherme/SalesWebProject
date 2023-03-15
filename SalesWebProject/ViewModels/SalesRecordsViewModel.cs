using SalesWebProject.Enums;
using SalesWebProject.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesWebProject.ViewModels
{
    public class SalesRecordsViewModel
    {

        public class SalesRecordsMainViewModel
        {
            public SalesRecordsMainViewModel()
            {
                Filter = new SalesRecordFilter();
            }

            public List<SaleRecordGroupedViewModel> SalesRecordsGroups { get; set; }

            public SalesRecordFilter Filter { get; set; }
        }

        public class SaleRecordGroupedViewModel
        {
            public string Name { get; set; }

            public int Counter { get; set; }

            public List<SalesRecordsViewModel> SalesRecords { get; set; }
        }
        public int Id { get; set; }

        [Display(Name = "Data")]
        public DateTime Date { get; set; }

        public string DateString { get { return this.Date.ToShortDateString(); } }

        [Display(Name = "Preço")]
        public double Amount { get; set; }

        public SaleStatusEnum Status { get; set; }

        public int SellerId { get; set; }

        public Seller Seller { get; set; }
    }

    public class SalesRecordFilter
    {
        public SalesRecordFilter()
        {
            DateEnd = DateTime.Now;
            DateStart = DateTime.Now.AddYears(-5);
            Status = false;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateTime? DateStart { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateTime? DateEnd { get; set; }
        public bool Status { get; set; }
        public bool AllPeriods { get; set; }
    }
}
