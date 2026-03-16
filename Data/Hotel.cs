using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Data
{
    //Code First approach - Using Data Annotations for validation and schema definition
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        //[StringLength(100)]
        public string HotelName { get; set; }

        [Required]
        [Range(1,5)]
        public int Rating { get; set; }
        [Required]
        [StringLength(500)]              
        public string Address { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set;}

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
    }
}
