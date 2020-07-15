using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PosgradoBot.Common.Model.Qualification
{
    public class QualificationModel
    {
        public string id { get; set; }
        public string idUser { get; set; }
        public string qualification { get; set; }
        public DateTime registerDate { get; set; }

    }
}
