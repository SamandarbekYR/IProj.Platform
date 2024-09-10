using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProj.Domain.Entities.Messages
{
    [Table("messages")]
    public class Message : BaseEntity
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string MessageContent { get; set; } = string.Empty;
        public DateTime SendTime { get; set; }
        public DateTime? ReadTime { get; set; }
        public bool? IsRead { get; set; }
    }
}
