using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace PharmaProject.Models
{
    public class Apply
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        // Educational Details
        public string Degree { get; set; }
        public string InstName { get; set; }
        public string Skills { get; set; }
        public string WorkExp { get; set; }
        public string UploadedFilePath {  get; set; }
        public bool IsPending { get; set; } = false;
        public int UserId { get; set; }



    }
}
