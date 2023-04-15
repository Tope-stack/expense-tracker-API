using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models.DTO
{
    public class ChangePasswordDTO
    {
        [Required]
        //[JsonProperty("Email")]
        [EmailAddress]

        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        //[JsonProperty("CurrentPassword")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        //[JsonProperty("NewPassword")]
        public string NewPassword { get; set; }

        [Required]
        //[JsonProperty("ConfirmPassword")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
