using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using PosgradoBot.Common.Cards;
using PosgradoBot.Data;
using PosgradoBot.Dialogs.CreateAppointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PosgradoBot.Dialogs.Curses
{
    public class CursesDialog : ComponentDialog
    {
        private IDataBaseService _databaseService;
        public CursesDialog(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
            var waterfallSteps = new WaterfallStep[]
            {
                ToShowButton,
                ValidateOption,
                SelectedOption
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
        }

        private async Task<DialogTurnResult> SelectedOption(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //Preinscripcionª1
            string selected = stepContext.Context.Activity.Text;
            string[] dtr = selected.Split('~');
            if (dtr[0] == "Preinscripcion")
            {
                switch (dtr[1])
                {
                    case "11":
                        string curse = "0";
                        return await stepContext.BeginDialogAsync(nameof(PreinscriptionDialog), curse, cancellationToken: cancellationToken);
                    case "21":
                        string curse2 = "1";
                        return await stepContext.BeginDialogAsync(nameof(PreinscriptionDialog), curse2, cancellationToken: cancellationToken);
                    case "22":
                        string curse3 = "2";
                        return await stepContext.BeginDialogAsync(nameof(PreinscriptionDialog), curse3, cancellationToken: cancellationToken);
                    case "23":
                        string curse4 = "3";
                        return await stepContext.BeginDialogAsync(nameof(PreinscriptionDialog), curse4, cancellationToken: cancellationToken);
                    case "31":
                        string curse5 = "4";
                        return await stepContext.BeginDialogAsync(nameof(PreinscriptionDialog), curse5, cancellationToken: cancellationToken);
                    case "32":
                        string curse6 = "5";
                        return await stepContext.BeginDialogAsync(nameof(PreinscriptionDialog), curse6, cancellationToken: cancellationToken);
                    case "33":
                        string curse7 = "6";
                        return await stepContext.BeginDialogAsync(nameof(PreinscriptionDialog), curse7, cancellationToken: cancellationToken);
                    default:
                        break;
                }
            }

            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> ToShowButton(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
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
            var reply = MessageFactory.Text("Selecciona el nivel de curso:");
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){Title = "Taller", Value = "Taller", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "Diplomado", Value = "Diplomado", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "Maestría", Value = "Maestria", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "Doctorado", Value = "Doctorado", Type = ActionTypes.ImBack},

                }
            };
            return reply as Activity;
        }

        private async Task<DialogTurnResult> ValidateOption(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = stepContext.Context.Activity.Text;
            switch (options)
            {
                case "Taller":
                    //return await TallerCards.ToShow(stepContext, cancellationToken);
                    await stepContext.Context.SendActivityAsync("Actualmente no tenemos cursos disponibles para Talleres...");
                    return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
                case "Diplomado":
                    return await DiplomadoCards.ToShow(stepContext, cancellationToken);
                case "Maestria":
                    return await MaestriaCards.ToShow(stepContext, cancellationToken);
                case "Doctorado":
                    await stepContext.Context.SendActivityAsync("Actualmente no tenemos cursos disponibles para Doctorados...");
                    return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
                default:
                    break;
            }
            //await stepContext.Context.SendActivityAsync($"Gracias por tu {options}", cancellationToken: cancellationToken);
            //await Task.Delay(1000);
            //await stepContext.Context.SendActivityAsync("¿En qué más te puedo ayudar?", cancellationToken: cancellationToken);
            //Save Qualification
            //await SaveQualification(stepContext, options);

            //original
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        //private async Task SaveQualification(WaterfallStepContext stepContext, string options)
        //{
        //    var qualifactionModel = new QualificationModel();
        //    qualifactionModel.id = Guid.NewGuid().ToString();
        //    qualifactionModel.idUser = stepContext.Context.Activity.From.Id;
        //    qualifactionModel.qualification = options;
        //    qualifactionModel.registerDate = DateTime.Now.Date;

        //    await _databaseService.Qualification.AddAsync(qualifactionModel);
        //    await _databaseService.SaveAsync();
        //}

        //private async Task CurseSelected(WaterfallStepContext stepContext)
        //{
        //    var clicked = stepContext.Context.Activity.Text;
        //}
    }
}
