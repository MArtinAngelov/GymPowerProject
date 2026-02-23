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
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer";
        public string FitnessGoal { get; set; } = "Maintenance";
        public int Age { get; set; }
        public string Gender { get; set; } = "Not Specified"; 
        public string TrainingLevel { get; set; } = "Beginner";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public bool IsActive { get; set; } = true;
    }
}
