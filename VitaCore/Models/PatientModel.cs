using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using VitaCore.Data;
using Xunit;

namespace VitaCore.Models
{
    [Table("patients")]
    public class PatientModel
    {
        [Column("id")]
        public int id { get; set; }

        [Column("user_id")]
        [Required]
        public int UserId { get; set; }

        [Column("age")]
        [Range(0, 120, ErrorMessage = "Age must be between 0 and 120.")]
        public int? age { get; set; }

        [Column("cnp")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "CNP must be exactly 13 characters.")]
        [RegularExpression("^[0-9]{13}$", ErrorMessage = "CNP must be numeric and 13 digits.")]
        public string? cnp { get; set; }

        [Column("adress_street")]
        [StringLength(100)]
        public string? address_street { get; set; }

        [Column("adress_city")]
        [StringLength(50)]
        public string? address_city { get; set; }

        [Column("adress_county")]
        [StringLength(50)]
        public string? address_county { get; set; }

        [Column("phone_number")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain digits only.")]
        public string? phone_number { get; set; }

        [Column("email")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? email { get; set; }

        [Column("occupation")]
        [StringLength(100)]
        public string? occupation { get; set; }

        [Column("workplace")]
        [StringLength(100)]
        public string? workplace { get; set; }

        [ValidateNever]
        public ICollection<MedicalHistoryModel> MedicalHistories { get; set; }

        [ValidateNever]
        public ICollection<RecommendationModel> Recommendations { get; set; }

        [ValidateNever]
        public ICollection<AlarmModel> Alarms { get; set; }

        [ValidateNever]
        public ICollection<PhysicalActivityModel> PhysicalActivities { get; set; }

        [ValidateNever]
        public ICollection<EcgSignalModel> EcgSignals { get; set; }

        [ValidateNever]
        public ICollection<SensorDataModel> SensorDatas { get; set; }

        [ValidateNever]
        public ICollection<LocationMapModel> LocationMaps { get; set; }

        [ValidateNever]
        public User User { get; set; }
    }

    public class PatientDto
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string? Cnp { get; set; }
        public string UserId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AdressStreet { get; set; }
        public string? AdressCity { get; set; }
        public string? AdressCounty { get; set; }
        public string? Occupation { get; set; }
        public string? Workplace { get; set; }
    }

    public class PatientUpdateDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AdressStreet { get; set; }
        public string? AdressCity { get; set; }
        public string? AdressCounty { get; set; }
        public string? Occupation { get; set; }
        public string? Workplace { get; set; }
        public string? Cnp { get; set; }
    }
}
