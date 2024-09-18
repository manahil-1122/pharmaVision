using System.ComponentModel.DataAnnotations;

namespace PharmaProject.Models
{
    public class Capsules
    {
        [Key]
        public int Id { get; set; }
        public string CapName { get; set; }
        public string Output { get; set; }
        public string CapSize{ get; set; }
        public string MachDim{ get; set; }
        public string ShipWeight{ get; set; }


    }
}
