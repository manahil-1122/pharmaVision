using System.ComponentModel.DataAnnotations;

namespace PharmaProject.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        public string CompanyName { get; set; }
        public string Username {  get; set; }

        public string Password { get; set; }

        public string Role {  get; set; }


    }
}
