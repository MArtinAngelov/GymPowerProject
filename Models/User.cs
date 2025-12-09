using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GymPower.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "Customer";
        public string FitnessGoal { get; set; } = "Maintenance";
        
        // New Fitness Profile Fields
        public int Age { get; set; }
        public string Gender { get; set; } = "Not Specified"; 
        public string TrainingLevel { get; set; } = "Beginner"; // Beginner, Intermediate, Advanced

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Order>? Orders { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
