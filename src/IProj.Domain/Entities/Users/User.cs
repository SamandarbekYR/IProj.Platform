using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProj.Domain.Entities.Users
{
    [Table("users")]
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Gmail { get; set; } = string.Empty;
        public bool IsOnline { get; set; } = false;
    }
}
