using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VitaCore.Models
{
    [Table("messages")]
    public class MessageModel
    {
        [Column("id")]
        [Key]
        public int id { get; set; }

        [Column("sender_id")]
        [Required]
        public int sender_id { get; set; }

        [ForeignKey("receiver_id")]
        [Required]
        public int receiver_id { get; set; }

        [Column("message")]
        [Required]
        public string message { get; set; }

        [Column("sent_at")]
        public DateTime? sent_at { get; set; }

        [Column("is_read")]
        public bool? is_read { get; set; }

    }

    public class SendMessageDto
    {
        public string SenderGuid { get; set; }   // din AppUser.Id
        public int ReceiverId { get; set; }      // id din tabela Doctors
        public string Message { get; set; }
    }

    public class ChatMessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderGuid { get; set; } // ← aici!
        public int ReceiverId { get; set; }
        public string Message1 { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
