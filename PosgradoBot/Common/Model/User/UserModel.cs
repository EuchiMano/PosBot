using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PosgradoBot.Common.Model.User
{
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string idChannel { get; set; }
        public string userNameChannel { get; set; }
        public string channel { get; set; }
        public DateTime registerDate { get; set; }
    }
}
