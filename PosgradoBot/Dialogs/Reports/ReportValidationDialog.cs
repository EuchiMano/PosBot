using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using PosgradoBot.Common.Model.BotState;
using PosgradoBot.Common.Model.User;
using PosgradoBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PosgradoBot.Dialogs.Reports
{
    public class ReportValidationDialog : ComponentDialog
    {
        private readonly IDataBaseService _databaseService;
        public static UserModel newUserModel = new UserModel();

        private readonly IStatePropertyAccessor<BotStateModel> _userState;

        public ReportValidationDialog(IDataBaseService databaseService, UserState userState)
        {
            _userState = userState.CreateProperty<BotStateModel>(nameof(BotStateModel));
            _databaseService = databaseService;

            var waterfallStep = new WaterfallStep[]
            {
                AskPass,
                Confirmation,
                FinalProcess
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallStep));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
        }

        private async Task<DialogTurnResult> AskPass(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
                 nameof(TextPrompt),
                 new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa tus codigo:") },
                 cancellationToken
             );
        }

        private async Task<DialogTurnResult> Confirmation(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var codigo = stepContext.Context.Activity.Text;
            if (codigo == "$RePoRtS")
            {
                //var preInscriptionModel = await _databaseService.
                //string summaryPreinscription = $"Apellidos: {preIncriptionModel.lastName}" +
                //    $"{Environment.NewLine} Nombres: {preIncriptionModel.name}";

                //await stepContext.Context.SendActivityAsync(summaryPreinscription, cancellationToken: cancellationToken);
                //await Task.Delay(1000);
                //await stepContext.Context.SendActivityAsync("¿En que mas puedo ayudarte?", cancellationToken: cancellationToken);
                return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
            }

            await stepContext.Context.SendActivityAsync("El codigo es incorrecto. Vuelve a Intentarlo", cancellationToken: cancellationToken);
            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> FinalProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userConfirmation = stepContext.Context.Activity.Text;

            if (userConfirmation.ToLower().Equals("si"))
            {
                //Save Database
                //string userId = stepContext.Context.Activity.From.Id;
                
                //var newInscriptionModel = new PreinscriptionModel();
                //newInscriptionModel.id = Guid.NewGuid().ToString();
                //newInscriptionModel.idUser = userId;
                //newInscriptionModel.date = DateTime.Now.Date;
                //newInscriptionModel.lastName = preIncriptionModel.lastName;
                //newInscriptionModel.name = preIncriptionModel.name;
                //newInscriptionModel.celular = preIncriptionModel.celular;
                //newInscriptionModel.correo = preIncriptionModel.correo;
                //newInscriptionModel.ci = preIncriptionModel.ci;
                //newInscriptionModel.curso = preIncriptionModel.curso;

                //await _databaseService.Preinscription.AddAsync(newInscriptionModel);
                //await _databaseService.SaveAsync();

                //await stepContext.Context.SendActivityAsync("Tu preinscripcion se guardo con exito.", cancellationToken: cancellationToken);
                

                ////Show Summary
                //string summaryPreinscription = $"Apellidos: {preIncriptionModel.lastName}" +
                //    $"{Environment.NewLine} Telefono: {preIncriptionModel.celular}" +
                //    $"{Environment.NewLine} Correo: {preIncriptionModel.correo}" +
                //    $"{Environment.NewLine} Ci: {preIncriptionModel.ci}" +
                //    $"{Environment.NewLine} Curso: {preIncriptionModel.curso}" +
                //    $"{Environment.NewLine} Nombres: {preIncriptionModel.name}";

                //await stepContext.Context.SendActivityAsync(summaryPreinscription, cancellationToken: cancellationToken);
                //await Task.Delay(1000);
                //await stepContext.Context.SendActivityAsync("¿En que mas puedo ayudarte?", cancellationToken: cancellationToken);
                //preIncriptionModel = new PreinscriptionModel();
            }
            else
            {
                await stepContext.Context.SendActivityAsync("No hay problema, sera la proxima", cancellationToken: cancellationToken);
            }

            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        }

        //private Activity CreateButtonConfirmation()
        //{
        //    var reply = MessageFactory.Text("¿Confirmas la creación de este formulario?");
        //    reply.SuggestedActions = new SuggestedActions()
        //    {
        //        Actions = new List<CardAction>()
        //        {
        //            new CardAction(){Title = "Si", Value = "Si", Type = ActionTypes.ImBack},
        //            new CardAction(){Title = "No", Value = "No", Type = ActionTypes.ImBack},
        //        }
        //    };
        //    return reply as Activity;
        //}
    }
}
