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
        public DateTime date { get; set; }
        public string lastName { get; set; }
        public string name { get; set; }
        public string ci { get; set; }
        public string correo { get; set; }
        public string celular { get; set; }
    }
}
