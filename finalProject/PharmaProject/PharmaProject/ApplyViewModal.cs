namespace PharmaProject
{
    public class ApplyViewModal
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        // Educational Details
        public string Degree { get; set; }
        public string InstName { get; set; }
        public string Skills { get; set; }
        public string WorkExp { get; set; }
        public IFormFile UploadedFile { get; set; }

    }
}
