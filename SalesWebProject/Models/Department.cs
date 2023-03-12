using SalesWebProject.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesWebProject.Models
{
    [Table("Department")]
    public class Department
    {
        public Department()
        {

        }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;

        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Seller> Sellers { get; set; }
    }
}
