using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PosgradoBot.Common.Model.Qualification
{
    public class QualificationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int idUser { get; set; }
        public string qualification { get; set; }
        public DateTime registerDate { get; set; }

    }
}
