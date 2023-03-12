using SalesWebProject.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesWebProject.Models
{
    [Table("SalesRecord")]
    public class SalesRecord
    {
        public SalesRecord()
        {

        }

        public SalesRecord(int id, DateTime date, double amount, SaleStatusEnum status, Seller seller)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Status = status;
            Seller = seller;
        }

        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public SaleStatusEnum Status { get; set; }

        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        public Seller Seller { get; set; }
    }
}
