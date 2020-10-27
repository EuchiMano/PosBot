using Microsoft.Azure.Cosmos;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using PosgradoBot.Common.Model.Qualification;
using PosgradoBot.Common.Model.User;
using PosgradoBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PosgradoBot.Dialogs.Qualification
{
    public class QualificationDialog: ComponentDialog
    {
        private IDataBaseService _databaseService;
        //private DataBaseService _databaseService1;

        public QualificationDialog(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
            var waterfallSteps = new WaterfallStep[]
            {
                ToShowButton,
                ValidateOption
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
        }

        private async Task<DialogTurnResult> ToShowButton(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //var uri = "https://posbot-cosmos.documents.azure.com:443/";
            //var key = "WPkM156Q8wnn1NGLXnB9tLnPea1gIvPccDFKSHoNDbRcSNVIuY2Hgrw7hzdGY7MPB8Aad7M1JUXbMNy4bc6aug==";
            //var db = "botdb";
            //var cl = "User";
            //List<UserModel> aea = new List<UserModel>();
            ////aea = _databaseService.QueryAllDocument(uri,key,db,cl);
            //aea = _databaseService.QueryAllDocument(uri,key,db,cl);



            return await stepContext.PromptAsync(
                nameof(TextPrompt), 
                new PromptOptions
                {
                    Prompt = CreateButtonsQualification()
                },
                cancellationToken
            );
        }

        private Activity CreateButtonsQualification()
        {
            var reply = MessageFactory.Text("Califícame por favor");
            reply.SuggestedActions = new SuggestedActions() 
            { 
                Actions = new List<CardAction>()
                {
                    new CardAction(){Title = "1⭐", Value = "1", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "2⭐", Value = "2", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "3⭐", Value = "3", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "4⭐", Value = "4", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "5⭐", Value = "5", Type = ActionTypes.ImBack},
                }   
            };
            return reply as Activity;
        }

        private async Task<DialogTurnResult> ValidateOption(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = stepContext.Context.Activity.Text;
            await stepContext.Context.SendActivityAsync($"Gracias por tus {options} ⭐", cancellationToken : cancellationToken);
            await Task.Delay(1000);
            await stepContext.Context.SendActivityAsync("¿En qué más te puedo ayudar?", cancellationToken: cancellationToken);
            //Save Qualification
            await SaveQualification(stepContext, options);
            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task SaveQualification(WaterfallStepContext stepContext, string options)
        {
            var id = stepContext.Context.Activity.From.Id;
            var userlast = await _databaseService.User.FirstOrDefaultAsync(x => x.idChannel == id);

            var qualifactionModel = new QualificationModel();
            qualifactionModel.idUser = userlast.id;
            qualifactionModel.qualification = options;
            qualifactionModel.registerDate = DateTime.Now;

            _databaseService.Qualification.Add(qualifactionModel);
            _databaseService.SaveChangesSQL();

        }

        
    }
}
