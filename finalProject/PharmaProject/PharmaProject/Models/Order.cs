using System.ComponentModel.DataAnnotations;

namespace PharmaProject.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string CompName { get; set; }

        public string MedType { get; set; }

        public string Name { get; set; }

        public string size {  get; set; }
        public string weight { get; set; }
        public int UserId { get; set; }
    

}
}
