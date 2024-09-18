using System.ComponentModel.DataAnnotations;

namespace PharmaProject.Models
{
    public class Tablets
    {
        [Key]

        public int Id { get; set; }

        public string MedName { get; set; }
        public string ModelNum { get; set; }

        public string Dies { get; set; }
        public string MaxPres { get; set; }

        public string Diameter {  get; set; }
        public string Depth { get; set; }   
        
        public string ProdCapacity { get; set; }

        public string MachineSize { get; set;}
        public string NetWeight { get; set; }

    }
}
