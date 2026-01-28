using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    [Table("Accounts")]
    public class AccountModel : BaseModel
    {
        public string Nickname { get; set; } = "";
        public string Account { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
