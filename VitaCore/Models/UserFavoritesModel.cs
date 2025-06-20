using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitaCore.Models
{
    [Table("user_favorites")]
    public class UserFavoritesModel 
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Required]
        [Column("user_id")]
        public string user_id { get; set; }

        [Required]
        [Column("doctor_id")]
        public int doctor_id { get; set; }

        // RELAȚII DE NAVIGARE:
        [ForeignKey("doctor_id")]
        public DoctorModel Doctor { get; set; }
    }
}
