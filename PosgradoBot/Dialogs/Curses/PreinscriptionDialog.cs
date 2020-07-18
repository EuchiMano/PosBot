using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using PosgradoBot.Common.Model.BotState;
using PosgradoBot.Common.Model.Preinscription;
using PosgradoBot.Common.Model.User;
using PosgradoBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PosgradoBot.Dialogs.Curses
{
    public class PreinscriptionDialog: ComponentDialog
    {
        private readonly IDataBaseService _databaseService;
        public static UserModel newUserModel = new UserModel();
        public static PreinscriptionModel preIncriptionModel = new PreinscriptionModel();

        private readonly IStatePropertyAccessor<BotStateModel> _userState;

        public PreinscriptionDialog(IDataBaseService databaseService, UserState userState)
        {
            _userState = userState.CreateProperty<BotStateModel>(nameof(BotStateModel));
            _databaseService = databaseService;
            

            var waterfallStep = new WaterfallStep[]
            {
                SetLastName,
                SetNames,
                SetCi,
                SetCorreo,
                SetCelular,
                Confirmation,
                FinalProcess
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallStep));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
        }

        private async Task<DialogTurnResult> SetCelular(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (userStateModel.medicalData)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                var correo = stepContext.Context.Activity.Text;
                preIncriptionModel.correo = correo;

                return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa tu numero de celular o telefono:") },
                    cancellationToken
                );
            }
        }

        private async Task<DialogTurnResult> SetCorreo(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (userStateModel.medicalData)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                var ci = stepContext.Context.Activity.Text;
                preIncriptionModel.ci = ci;

                return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa tu correo:") },
                    cancellationToken
                );
            }
        }

        private async Task<DialogTurnResult> SetCi(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (userStateModel.medicalData)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                var fullName = stepContext.Context.Activity.Text;
                preIncriptionModel.name = fullName;

                return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa tu número de carnet de identidad:") },
                    cancellationToken
                );
            }
        }

        private async Task<DialogTurnResult> SetNames(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (userStateModel.medicalData)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                var lastName = stepContext.Context.Activity.Text;
                preIncriptionModel.lastName = lastName;

                return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa tu nombre completo:") },
                    cancellationToken
                );
            }
        }

        private async Task<DialogTurnResult> SetLastName(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var lastName = stepContext.Context.Activity.Text;
            string msgFromPreviousDialog = (string)stepContext.ActiveDialog.State["options"];
            preIncriptionModel.curso = msgFromPreviousDialog;

            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (userStateModel.medicalData)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa tus apellidos:") },
                cancellationToken
            );
            }
        }

        private async Task<DialogTurnResult> Confirmation(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var celular = stepContext.Context.Activity.Text;
            preIncriptionModel.celular = celular;

            return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions { Prompt = CreateButtonConfirmation() },
                cancellationToken
            );
        }

        private async Task<DialogTurnResult> FinalProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userConfirmation = stepContext.Context.Activity.Text;

            if (userConfirmation.ToLower().Equals("si"))
            {
                //Save Database
                string userId = stepContext.Context.Activity.From.Id;
                //var preInscriptionModel = await _databaseService.Preinscription.FirstOrDefaultAsync(x => x.id == userId);

                //var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());

                //if (!userStateModel.medicalData)
                //{
                //    //Update User
                //    userModel.lastName = newUserModel.lastName;
                //    userModel.name = newUserModel.name;
                //    userModel.ci = newUserModel.ci;
                //    userModel.correo = newUserModel.correo;
                //    userModel.celular = newUserModel.celular;

                //    _databaseService.User.Update(userModel);
                //    await _databaseService.SaveAsync();
                //}

                //var user = await _databaseService.User.FirstOrDefaultAsync(x => x.id == turnContext.Activity.From.Id);
                //if (user == null)
                //{
                //    await _databaseService.User.AddAsync(userModel);
                //    await _databaseService.SaveAsync();
                //}


                //Save Preinscription
                var newInscriptionModel = new PreinscriptionModel();
                newInscriptionModel.id = Guid.NewGuid().ToString();
                newInscriptionModel.idUser = userId;
                newInscriptionModel.date = DateTime.Now.Date;
                newInscriptionModel.lastName = preIncriptionModel.lastName;
                newInscriptionModel.name = preIncriptionModel.name;
                newInscriptionModel.celular = preIncriptionModel.celular;
                newInscriptionModel.correo = preIncriptionModel.correo;
                newInscriptionModel.ci = preIncriptionModel.ci;
                newInscriptionModel.curso = preIncriptionModel.curso;

                await _databaseService.Preinscription.AddAsync(newInscriptionModel);
                await _databaseService.SaveAsync();

                await stepContext.Context.SendActivityAsync("Tu preinscripcion se guardo con exito.", cancellationToken: cancellationToken);
                //userStateModel.medicalData = true;

                //Show Summary
                string summaryPreinscription = $"Apellidos: {preIncriptionModel.lastName}" +
                    $"{Environment.NewLine} Telefono: {preIncriptionModel.celular}" +
                    $"{Environment.NewLine} Correo: {preIncriptionModel.correo}" +
                    $"{Environment.NewLine} Ci: {preIncriptionModel.ci}" +
                    $"{Environment.NewLine} Curso: {preIncriptionModel.curso}" +
                    $"{Environment.NewLine} Nombres: {preIncriptionModel.name}";

                await stepContext.Context.SendActivityAsync(summaryPreinscription, cancellationToken: cancellationToken);
                await Task.Delay(1000);
                await stepContext.Context.SendActivityAsync("¿En que mas puedo ayudarte?", cancellationToken: cancellationToken);
                preIncriptionModel = new PreinscriptionModel();
            }
            else
            {
                await stepContext.Context.SendActivityAsync("No hay problema, sera la proxima", cancellationToken: cancellationToken);
            }

            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        }

        private Activity CreateButtonConfirmation()
        {
            var reply = MessageFactory.Text("¿Confirmas la creación de este formulario?");
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){Title = "Si", Value = "Si", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "No", Value = "No", Type = ActionTypes.ImBack},
                }
            };
            return reply as Activity;
        }
    }
}
