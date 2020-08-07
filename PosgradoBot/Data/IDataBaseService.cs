using Microsoft.EntityFrameworkCore;
using PosgradoBot.Common.Model;
using PosgradoBot.Common.Model.Curses;
using PosgradoBot.Common.Model.Preinscription;
using PosgradoBot.Common.Model.Qualification;
using PosgradoBot.Common.Model.Tickets;
using PosgradoBot.Common.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PosgradoBot.Data
{
    public interface IDataBaseService
    {
        DbSet<UserModel> User { get; set; }
        DbSet<QualificationModel> Qualification { get; set; }
        DbSet<PreinscriptionModel> Preinscription { get; set; }
        DbSet<TicketModel> Ticket { get; set; }
        DbSet<Curses> Curses { get; set; }
        DbSet<PaysModel> Pays { get; set; }
        Task<bool> SaveAsync();
        //Task<List<UserModel>> QueryItemsAsync();
        //List<UserModel> QueryAllDocument(string Uri, string Key, string DatabaseName, string CollectionName);
    }
}
