// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EmptyBot v4.9.2

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PosgradoBot.Data;
using PosgradoBot.Dialogs;
using PosgradoBot.Infrastructure.Luis;
using PosgradoBot.Infrastructure.SendGridEmail;
using SendGrid;

namespace PosgradoBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var storage = new AzureBlobStorage(
                Configuration.GetSection("StorageConnectionsString").Value,
                Configuration.GetSection("StorageContainer").Value
                );
            var userState = new UserState(storage);
            services.AddSingleton(userState);
            services.AddControllers().AddNewtonsoftJson();

            var conversationState = new ConversationState(storage);
            services.AddSingleton(conversationState);

            //services.AddDbContext<DataBaseService>(options =>
            //{
            //    options.UseCosmos(
            //        Configuration["CosmosEndPoint"],
            //        Configuration["CosmosKey"],
            //        Configuration["CosmosDatabase"]
            //        );
            //});

            services.AddDbContext<DataBaseService>(options => options.UseSqlServer(
             Configuration.GetConnectionString("BloggingDatabase")));

            //UserID = posgradobot; Password = Posbot123;
            services.AddScoped<IDataBaseService, DataBaseService>();

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            services.AddSingleton<ISendGridEmailService, SendGridEmailService>();
            services.AddSingleton<ILuisService, Infrastructure.Luis.LuisService>();
            services.AddTransient<RootDialog>();
            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, PosBot<RootDialog>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
