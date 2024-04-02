using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Database.Models
{
    public class UserDbModel
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("role")]
        public UserRole Role { get; set; }

        [Required]
        [Column("email", TypeName = "varchar(100)")]
        public string Email { get; set; }

        [Required]
        [Column("password_hashed", TypeName = "varchar(100)")]
        public string PasswordHashed { get; set; }

        public UserDbModel(Guid id, UserRole role, string email, string passwordHashed)
        {
            Id = id;
            Role = role;
            Email = email;
            PasswordHashed = passwordHashed;
        }
    }
}