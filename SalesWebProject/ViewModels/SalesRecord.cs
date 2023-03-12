using SalesWebProject.Enums;
using SalesWebProject.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesWebProject.ViewModels
{
    public class SalesRecordViewModel
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public SaleStatusEnum Status { get; set; }

        public int SellerId { get; set; }

        public Seller Seller { get; set; }
    }
}
