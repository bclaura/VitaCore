using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VitaCore.Data;

namespace VitaCore.Models
{
    [Table("doctors")]
    public class DoctorModel
    {
        [Column("id")]
        [Key]
        public int id { get; set; }

        [Column("user_id")]
        [Required]
        public int UserId { get; set; }

        [Column("honorific_title")]
        [StringLength(20)]
        public string? honorific_title { get; set; }

        [Column("gender")]
        [StringLength(10)]
        public string? gender { get; set; }

        [Column("is_favorite")]
        public bool? is_favorite { get; set; }

        [Column("bio")]
        public string? bio { get; set; }

        [Column("availability_hours")]
        [StringLength(255)]
        public string? availability_hours { get; set; }

        [Column("clinic_address")]
        [StringLength(255)]
        public string? clinic_address { get; set; }

        [Column("profile_picture_base64")]
        public string? profile_picture_base64 { get; set; }


        [Required]
        [StringLength(255)]
        public string Specialization { get; set; }

        [ValidateNever]
        public User User { get; set; }


        [NotMapped]
        public string FullName => $"{User?.FirstName} {User?.LastName}".Trim();

        [NotMapped]
        public string LastName => User?.LastName ?? string.Empty;

    }

    public class DoctorMauiDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string LastName { get; set; }
        public string HonorificTitle { get; set; }
        public string Gender { get; set; }
        public bool IsFavorite { get; set; }
        public string Bio { get; set; }
        public string AvailabilityHours { get; set; }
        public string ClinicAddress { get; set; }
        public string Specialization { get; set; }
        public string ProfilePictureBase64 { get; set; }
    }
}