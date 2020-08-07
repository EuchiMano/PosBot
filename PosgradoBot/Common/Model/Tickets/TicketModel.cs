using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PosgradoBot.Common.Model.Tickets
{
    public class TicketModel
    {
        public string id { get; set; }
        public string idUser { get; set; }
        public string description { get; set; }
        public DateTime registerDate  { get; set; }
    }
}
