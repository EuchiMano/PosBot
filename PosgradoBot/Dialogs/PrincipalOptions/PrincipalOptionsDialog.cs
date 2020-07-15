using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using PosgradoBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PosgradoBot.Dialogs.OptionsPrincipal
{
    public class PrincipalOptionsDialog: ComponentDialog
    {
        private IDataBaseService _databaseService;
        public PrincipalOptionsDialog(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
            var waterfallSteps = new WaterfallStep[]
            {
                ToShowButton,
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
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
            var reply = MessageFactory.Text("Estas son algunas opciones con las que te puedo ayudar:");
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){Title = "Cursos Disponibles", Value = "Cursos", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "Información sobre la Escuela", Value = "Info", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "Consulta de Pagos", Value = "Pagos", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "Atención Personalizada", Value = "Atencion", Type = ActionTypes.ImBack},
                }
            };
            return reply as Activity;
        }

        //private async Task<DialogTurnResult> ValidateOption(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //{
        //    var options = stepContext.Context.Activity.Text;
        //    switch (options)
        //    {
        //        case "Cursos":
        //            return await TallerCards.ToShow(stepContext, cancellationToken);
        //        case "Diplomado":
        //            return await DiplomadoCards.ToShow(stepContext, cancellationToken);
        //        case "Maestria":
        //            return await MaestriaCards.ToShow(stepContext, cancellationToken);
        //        case "Doctorado":
        //            await stepContext.Context.SendActivityAsync("Actualmente no tenemos cursos disponibles para Doctorados...");
        //            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        //        default:
        //            break;
        //    }
        //    return await stepContext.NextAsync(cancellationToken: cancellationToken);
        //}

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
