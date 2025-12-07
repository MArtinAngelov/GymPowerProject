using System;
using System.ComponentModel.DataAnnotations;

namespace GymPower.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        
        [Required]
        public string Role { get; set; } = string.Empty; // "User" or "Assistant"

        [Required]
        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
