using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PosgradoBot.Common.Model.Preinscription
{
    public class PreinscriptionModel
    {
        public string id { get; set; }
        public string idUser { get; set; }
        public string idCurse { get; set; }
        public string apellidoP { get; set; }
        public string apellidoM { get; set; }
        public string names { get; set; }
        public string ci { get; set; }
        public string ciemi { get; set; }
        public string correo { get; set; }
        public string celular { get; set; }
        public string fechanac { get; set; }
        public string lugarnac { get; set; }
        public string direccion { get; set; }
        public string profesion { get; set; }
        public string estcivil { get; set; }
        public string grado { get; set; }
        public string fuerza { get; set; }
        public string codigoexemi { get; set; }
        public string excarrera { get; set; }
        public string fechaegreso { get; set; }
    }
}
