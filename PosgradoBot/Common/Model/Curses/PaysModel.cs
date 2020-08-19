using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PosgradoBot.Common.Model.Curses
{
    public class PaysModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string ci { get; set; }
        public string apellidos { get; set; }
        public string nombres { get; set; }
        public double cuotaUno { get; set; }
        public double cuotaDos { get; set; }
        public double cuotaTres { get; set; }
        public double cuotaCuatro { get; set; }
        public double cuotaCinco { get; set; }
        public double cuotaSeis { get; set; }
        public string idCurso { get; set; }
    }
}
