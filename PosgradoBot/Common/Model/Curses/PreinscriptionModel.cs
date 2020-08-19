using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PosgradoBot.Common.Model.Preinscription
{
    public class PreinscriptionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int idUser { get; set; }
        public int idCurse { get; set; }
        public string apellidoP { get; set; }
        public string apellidoM { get; set; }
        public string names { get; set; }
        public string ci { get; set; }
        public string celular { get; set; }
        public string grado { get; set; }
        public string fuerza { get; set; }
        public string codigoexemi { get; set; }
        public string excarrera { get; set; }
        public string fechaegreso { get; set; }
    }
}
