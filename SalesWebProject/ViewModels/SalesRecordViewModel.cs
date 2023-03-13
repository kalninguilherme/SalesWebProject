using SalesWebProject.Enums;
using SalesWebProject.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesWebProject.ViewModels
{
    public class SalesRecordViewModel
    {
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
}
