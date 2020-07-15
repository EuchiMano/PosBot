using Microsoft.EntityFrameworkCore;
using PosgradoBot.Common.Model.Preinscription;
using PosgradoBot.Common.Model.Qualification;
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
        Task<bool> SaveAsync();
    }
}
