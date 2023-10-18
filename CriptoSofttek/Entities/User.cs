﻿using System.ComponentModel.DataAnnotations;

namespace CriptoSofttek.Entities
{
    public class User
    {
        public User() { }

        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
