using System.ComponentModel.DataAnnotations;

namespace PharmaProject.Models
{
    public class Liquid
    {
        [Key]
        public int Id { get; set; }

        public string LiquidName { get; set; }

        public string AirPressure { get; set; }

        public string AirVolume { get; set; }

        public string FillSpeed { get; set; }

        public string FillRange { get; set; }
    }
}
