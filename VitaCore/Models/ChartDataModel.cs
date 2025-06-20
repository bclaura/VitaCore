using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VitaCore.Models
{
    [Table("chart_data")]
    public class ChartDataModel
    {
        [Column("id")]
        [Key]
        public int id { get; set; }

        [Column("patient_id")]
        [Required]
        public int patient_id { get; set; }

        [Column("chart_type")]
        [StringLength(50)]
        public string? char_type { get; set; }

        [Column("data_label")]
        [StringLength(225)]
        public string? data_label { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? value { get; set; }

        [Column("recorded_at")]
        public DateTime? recorded_at { get; set; }
    }
}
