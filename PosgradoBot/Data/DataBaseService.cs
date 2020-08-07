using AdaptiveExpressions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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
using System.Xml.Linq;

namespace PosgradoBot.Data
{
    public class DataBaseService : DbContext, IDataBaseService
    {
        //CosmosContainer 
        //private Container container;
       
        public DataBaseService(DbContextOptions options) : base(options)
        {
            Database.EnsureCreatedAsync();

        }

        public DataBaseService()
        {
            Database.EnsureCreatedAsync();
        }

        public DbSet<UserModel> User { get; set; }
        public DbSet<QualificationModel> Qualification { get; set; }
        public DbSet<PreinscriptionModel> Preinscription { get; set; }
        public DbSet<TicketModel> Ticket { get; set; }
        public DbSet<Curses> Curses { get; set; }
        public DbSet<PaysModel> Pays { get; set; }

        public async Task<bool> SaveAsync()
        {
            return (await SaveChangesAsync() > 0);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().ToContainer("User").HasPartitionKey("channel").HasNoDiscriminator().HasKey("id");
            modelBuilder.Entity<QualificationModel>().ToContainer("Qualification").HasPartitionKey("idUser").HasNoDiscriminator().HasKey("id");
            modelBuilder.Entity<PreinscriptionModel>().ToContainer("Preinscription").HasPartitionKey("idUser").HasNoDiscriminator().HasKey("id");
            modelBuilder.Entity<TicketModel>().ToContainer("Ticket").HasPartitionKey("idUser").HasNoDiscriminator().HasKey("id");
            modelBuilder.Entity<Curses>().ToContainer("Curses").HasPartitionKey("title").HasNoDiscriminator().HasKey("id");
            modelBuilder.Entity<PaysModel>().ToContainer("Pays").HasPartitionKey("ci").HasNoDiscriminator().HasKey("id");
        }



        //public async Task<List<UserModel>> QueryAllDocument()
        //{

        //    FeedIterator<UserModel> feedIterator = container.GetItemQueryIterator<UserModel>("select * from Ticket");

        //        while (feedIterator.HasMoreResults)
        //        {
        //            Microsoft.Azure.Cosmos.FeedResponse<UserModel> response = await feedIterator.ReadNextAsync();
        //            foreach (var item in response)
        //            {
        //                Console.WriteLine(item);
        //            }
        //        }
        //}

        //public async Task<List<UserModel>> QueryItemsAsync()
        //{


        //    var sqlQueryText = "SELECT * FROM c";
        //    QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
        //    FeedIterator<UserModel> queryResultSetIterator = this.container.GetItemQueryIterator<UserModel>(queryDefinition);

        //    List<UserModel> users = new List<UserModel>();

        //    while (queryResultSetIterator.HasMoreResults)
        //    {
        //        Microsoft.Azure.Cosmos.FeedResponse<UserModel> currentResultSet = await queryResultSetIterator.ReadNextAsync();
        //        foreach (UserModel user in currentResultSet)
        //        {
        //            users.Add(user);
        //        }
        //    }
        //    return users;
        //}

        //public List<UserModel> QueryAllDocument(string Uri, string Key, string DatabaseName, string CollectionName)
        //{
        //    DocumentClient client = new DocumentClient(new Uri(Uri), Key, new ConnectionPolicy { EnableEndpointDiscovery = false });
        //    List<UserModel> list = client.CreateDocumentQuery<UserModel>(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName).ToString(), new SqlQuerySpec("SELECT * FROM User"), new FeedOptions { MaxItemCount = -1 }).ToList<UserModel>();
        //    return list;
        //}

        //private static async Task QueryItemsAsync(CosmosClient cosmosClient)
        //{
        //    var sqlQueryText = "SELECT * FROM User";
        //    CosmosContainer container = 
        //}
    }
}
