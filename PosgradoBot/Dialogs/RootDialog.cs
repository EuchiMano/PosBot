using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using PosgradoBot.Common.Cards;
using PosgradoBot.Data;
using PosgradoBot.Dialogs.CreateAppointment;
using PosgradoBot.Dialogs.Curses;
using PosgradoBot.Dialogs.PersonalAtention;
using PosgradoBot.Dialogs.Qualification;
using PosgradoBot.Infrastructure.Luis;
using PosgradoBot.Infrastructure.QnAMakerAI;
using PosgradoBot.Infrastructure.SendGridEmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace PosgradoBot.Dialogs
{
    public class RootDialog : ComponentDialog
    {
        private readonly ILuisService _luisService;
        private readonly IDataBaseService _databaseService;
        private readonly ISendGridEmailService _sendGridEmailService;
        private readonly IQnAMakerAIService _qnaMakerAIService;

        public RootDialog(ILuisService luisService, IDataBaseService databaseService, UserState userState, ISendGridEmailService sendGridEmailService, IQnAMakerAIService qnaMakerAIService)
        {
            _qnaMakerAIService = qnaMakerAIService;
            _sendGridEmailService = sendGridEmailService;
            _databaseService = databaseService;
            _luisService = luisService;

            var waterfallSteps = new WaterfallStep[]
            {
                InitialProcess,
                FinalProcess
            };

            AddDialog(new PreinscriptionDialog(_databaseService, userState));
            AddDialog(new QualificationDialog(_databaseService));
            AddDialog(new PaysDialog(_databaseService));
            AddDialog(new AgentDialog(_databaseService, _sendGridEmailService));
            AddDialog(new CursesDialog(_databaseService));

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var luisResult = await _luisService._luisRecognizer.RecognizeAsync(stepContext.Context, cancellationToken);
            return await ManageIntentions(stepContext, luisResult, cancellationToken);
        }

        private async Task<DialogTurnResult> ManageIntentions(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            var topIntent = luisResult.GetTopScoringIntent();

            if (topIntent.score > 0.5)
            {
                switch (topIntent.intent)
                {
                    case "Saludar":
                        await IntentSaludar(stepContext, luisResult, cancellationToken);
                        break;
                    case "Despedirse":
                        await IntentAgradecer(stepContext, luisResult, cancellationToken);
                        break;
                    case "Calificar":
                        return await IntentCalificar(stepContext, luisResult, cancellationToken);
                    case "VerCursos":
                        return await IntentVerCursos(stepContext, luisResult, cancellationToken);
                    case "AtencionPersonal":
                        return await IntentAgente(stepContext, luisResult, cancellationToken);
                    case "Pagos":
                        return await IntentPagos(stepContext, luisResult, cancellationToken);
                    case "None":
                        await IntentNone(stepContext, luisResult, cancellationToken);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                await IntentNone(stepContext, luisResult, cancellationToken);
            }
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> IntentPagos(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(PaysDialog), cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> IntentAgente(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(AgentDialog), cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> IntentVerCursos(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(CursesDialog), cancellationToken: cancellationToken);
        }

        //private async Task<DialogTurnResult> IntentPreInscripcion(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        //{
        //    return await stepContext.BeginDialogAsync(nameof(CreateAppointmentDialog), cancellationToken: cancellationToken);
        //}

        #region IntentLuis
        private async Task<DialogTurnResult> IntentCalificar(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(QualificationDialog), cancellationToken: cancellationToken);
        }

        private async Task IntentSaludar(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync($"Hola que gusto verte, ¿te puedo ayudar en algo?", cancellationToken: cancellationToken);
        }

        private async Task IntentAgradecer(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Hasta luego, espero haber sido de ayuda.", cancellationToken: cancellationToken);
        }

        //private async Task IntentDespedir(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        //{
        //    await stepContext.Context.SendActivityAsync("Espero verte pronto", cancellationToken: cancellationToken);
        //}

        //private async Task IntentVerOpciones(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        //{
        //    await stepContext.Context.SendActivityAsync("Aqui tengo mis opciones", cancellationToken: cancellationToken);
        //    await MainOptionsCard.ToShow(stepContext, cancellationToken);
        //}

        private async Task IntentNone(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            var resultQnA = await _qnaMakerAIService._qnaMakerResult.GetAnswersAsync(stepContext.Context);
            var score = resultQnA.FirstOrDefault()?.Score;

            string response = resultQnA.FirstOrDefault()?.Answer;

            if (score >= 0.5)
            {
                await stepContext.Context.SendActivityAsync(response, cancellationToken: cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync("No entiendo lo que me dices.", cancellationToken: cancellationToken);
                //await Task.Delay(1000);
                //await IntentVerOpciones(stepContext, luisResult, cancellationToken);
                string summary2 = $"Puedo ayudarte con las siguiente opciones:" +
                    $"{Environment.NewLine} *Consulta de cursos" +
                    $"{Environment.NewLine} *Consulta de pagos" +
                    $"{Environment.NewLine} *Atencion Personalizada" +
                    $"{Environment.NewLine} *Preguntas Frecuentes sobre la EMI Posgrado";
                await stepContext.Context.SendActivityAsync(summary2, cancellationToken: cancellationToken);
            }
            //await stepContext.ContinueDialogAsync();
        }
        #endregion

        private async Task<DialogTurnResult> FinalProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
