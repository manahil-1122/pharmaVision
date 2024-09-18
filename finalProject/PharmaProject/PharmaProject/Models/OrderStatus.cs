using System.ComponentModel.DataAnnotations;

namespace PharmaProject.Models
{
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; } // Foreign key for Order
        public int UserId { get; set; } // UserId linked to Order
        public int OrdNum { get; set; }

        public string CompName { get; set; }

        public string MedType { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string Time { get; set; }




    }
}
