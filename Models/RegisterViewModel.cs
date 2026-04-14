using System.ComponentModel.DataAnnotations;

namespace GymPower.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Потребителското име е задължително.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Потребителското име трябва да бъде между {2} и {1} символа.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имейлът е задължителен.")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Паролата е задължителна.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Паролата трябва да бъде поне {2} символа.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моля, потвърдете паролата.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фитнес целта е задължителна.")]
        public string FitnessGoal { get; set; } = "Maintenance";
    }
}
