using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitaCore.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("first_name")]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [Column("last_name")]
        [MaxLength(255)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Column("password")]
        [MaxLength(255)]
        public string Password { get; set; } // stocată brut momentan

        [Required]
        [Column("role")]
        [MaxLength(20)]
        public string Role { get; set; } = "User"; // default

        [Column("mobile_number")]
        public string? MobileNumber { get; set; }

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("profile_picture_base64")]
        public string? ProfilePictureBase64 { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ❗ UtcNow pentru PostgreSQL compatibil

        // Pentru PostgreSQL, coloana `created_at` e `timestamp` → trebuie să te asiguri că e UTC în viitor
        // Pentru SQL Server, `datetime`/`datetime2` acceptă tot UTC dacă setezi manual
    }
}
