using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using PosgradoBot.Data;
using PosgradoBot.Dialogs.Curses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PosgradoBot.Dialogs.Reports
{
    public class ReportsDialog: ComponentDialog
    {
        private IDataBaseService _databaseService;
        public ReportsDialog(IDataBaseService databaseService)
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
                    case "1":
                        string curse = "Curso de Microsoft Teams y OneDrive";
                        return await stepContext.BeginDialogAsync(nameof(PreinscriptionDialog), curse, cancellationToken: cancellationToken);
                    case "2":
                        string curse2 = "Diplomado en Educación Superior Universitaria a Distancia";
                        return await stepContext.BeginDialogAsync(nameof(PreinscriptionDialog), curse2, cancellationToken: cancellationToken);
                    case "3":
                        string curse3 = "Diplomado en Planificación y Desarrollo de Competencias Profesionales en Educación Superior";
                        return await stepContext.BeginDialogAsync(nameof(PreinscriptionDialog), curse3, cancellationToken: cancellationToken);
                    case "4":
                        string curse4 = "Maestría en Administración de Empresas";
                        return await stepContext.BeginDialogAsync(nameof(PreinscriptionDialog), curse4, cancellationToken: cancellationToken);
                    case "5":
                        string curse5 = "Maestría en Recursos Naturales y Gestión Ambiental";
                        return await stepContext.BeginDialogAsync(nameof(PreinscriptionDialog), curse5, cancellationToken: cancellationToken);
                    case "6":
                        string curse6 = "Maestría en Educación Superior Universitaria";
                        return await stepContext.BeginDialogAsync(nameof(PreinscriptionDialog), curse6, cancellationToken: cancellationToken);
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
                    new CardAction(){Title = "Usuarios", Value = "Usuarios", Type = ActionTypes.ImBack},
                }
            };
            return reply as Activity;
        }

        private async Task<DialogTurnResult> ValidateOption(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = stepContext.Context.Activity.Text;
            switch (options)
            {
                case "Usuarios":
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

           //await _databaseService.Qualification.AddAsync(qualifactionModel);
        //    await _databaseService.SaveAsync();
        //}
    }
}
