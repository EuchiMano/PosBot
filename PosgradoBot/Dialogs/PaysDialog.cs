using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using PosgradoBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PosgradoBot.Dialogs
{
    public class PaysDialog : ComponentDialog
    {
        private IDataBaseService _databaseService;
        public PaysDialog(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
            var waterfallSteps = new WaterfallStep[]
            {
                CiNumber,
                ValidateOption
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
        }

        private async Task<DialogTurnResult> CiNumber(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
                 nameof(TextPrompt),
                 new PromptOptions { Prompt = MessageFactory.Text("Ok. Necesito que escribas tu número de carnet para buscarlo: ") },
                 cancellationToken
             );
        }

        private async Task<DialogTurnResult> ValidateOption(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = stepContext.Context.Activity.Text;
            var res = isNumber(options);

            if (!res)
            {
                await stepContext.Context.SendActivityAsync($"Hey, en este campo solo se permite numeros...", cancellationToken: cancellationToken);
                return await stepContext.ReplaceDialogAsync(nameof(PaysDialog), options: 0);
            }


            
            var payLast = await _databaseService.Pays.FirstOrDefaultAsync(x => x.ci == options);
            if (payLast != null)
            {
                var totalpay = payLast.cuotaUno + payLast.cuotaDos + payLast.cuotaTres + payLast.cuotaCuatro + payLast.cuotaCinco + payLast.cuotaSeis;


                if (totalpay == 3000.00)
                {
                    string summaryComplete = $"Apellidos: {payLast.apellidos}" +
                $"{Environment.NewLine} Nombres: {payLast.nombres}" +
                $"{Environment.NewLine} Ci: {payLast.ci}" +
                $"{Environment.NewLine} No debes ninguna cuota";
                    await stepContext.Context.SendActivityAsync(summaryComplete, cancellationToken: cancellationToken);
                    await stepContext.Context.SendActivityAsync("¿En qué más te puedo ayudar?. Si terminaste puedes escribir calificar para darme una puntuacion.", cancellationToken: cancellationToken);
                    return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
                }

                if (payLast.cuotaUno == 0.00)
                {
                    string summaryPreinscription = $"Apellidos: {payLast.apellidos}" +
                    $"{Environment.NewLine} Nombres: {payLast.nombres}" +
                    $"{Environment.NewLine} Ci: {payLast.ci}" +
                    $"{Environment.NewLine} Te encuentras en la 1era cuota de 6 del total de 3000 Bs";

                    await stepContext.Context.SendActivityAsync(summaryPreinscription, cancellationToken: cancellationToken);
                    await stepContext.Context.SendActivityAsync("¿En qué más te puedo ayudar?. Si terminaste puedes escribir calificar para darme una puntuacion.", cancellationToken: cancellationToken);
                    return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
                }
                if (payLast.cuotaDos == 0.00)
                {
                    string summaryPreinscription = $"Apellidos: {payLast.apellidos}" +
                    $"{Environment.NewLine} Nombres: {payLast.nombres}" +
                    $"{Environment.NewLine} Ci: {payLast.ci}" +
                    $"{Environment.NewLine} Te encuentras en la 2da cuota de 6 del total de 3000 Bs";

                    await stepContext.Context.SendActivityAsync(summaryPreinscription, cancellationToken: cancellationToken);
                    await stepContext.Context.SendActivityAsync("¿En qué más te puedo ayudar?. Si terminaste puedes escribir calificar para darme una puntuacion.", cancellationToken: cancellationToken);
                    return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
                }

                if (payLast.cuotaTres == 0.00)
                {
                    string summaryPreinscription = $"Apellidos: {payLast.apellidos}" +
                    $"{Environment.NewLine} Nombres: {payLast.nombres}" +
                    $"{Environment.NewLine} Ci: {payLast.ci}" +
                    $"{Environment.NewLine} Te encuentras en la 3era cuota de 6 del total de 3000 Bs";

                    await stepContext.Context.SendActivityAsync(summaryPreinscription, cancellationToken: cancellationToken);
                    await stepContext.Context.SendActivityAsync("¿En qué más te puedo ayudar?. Si terminaste puedes escribir calificar para darme una puntuacion.", cancellationToken: cancellationToken);
                    return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
                }

                if (payLast.cuotaCuatro == 0.00)
                {
                    string summaryPreinscription = $"Apellidos: {payLast.apellidos}" +
                    $"{Environment.NewLine} Nombres: {payLast.nombres}" +
                    $"{Environment.NewLine} Ci: {payLast.ci}" +
                    $"{Environment.NewLine} Te encuentras en la 4ta cuota de 6 del total de 3000 Bs";

                    await stepContext.Context.SendActivityAsync(summaryPreinscription, cancellationToken: cancellationToken);
                    await stepContext.Context.SendActivityAsync("¿En qué más te puedo ayudar?. Si terminaste puedes escribir calificar para darme una puntuacion.", cancellationToken: cancellationToken);
                    return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
                }

                if (payLast.cuotaCinco == 0.00)
                {
                    string summaryPreinscription = $"Apellidos: {payLast.apellidos}" +
                    $"{Environment.NewLine} Nombres: {payLast.nombres}" +
                    $"{Environment.NewLine} Ci: {payLast.ci}" +
                    $"{Environment.NewLine} Te encuentras en la 5ta cuota de 6 del total de 3000 Bs";

                    await stepContext.Context.SendActivityAsync(summaryPreinscription, cancellationToken: cancellationToken);
                    await stepContext.Context.SendActivityAsync("¿En qué más te puedo ayudar?. Si terminaste puedes escribir calificar para darme una puntuacion.", cancellationToken: cancellationToken);
                    return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
                }

                if (payLast.cuotaSeis == 0.00)
                {
                    string summaryPreinscription = $"Apellidos: {payLast.apellidos}" +
                    $"{Environment.NewLine} Nombres: {payLast.nombres}" +
                    $"{Environment.NewLine} Ci: {payLast.ci}" +
                    $"{Environment.NewLine} Te encuentras en la ultima cuota del total de 3000 Bs";

                    await stepContext.Context.SendActivityAsync(summaryPreinscription, cancellationToken: cancellationToken);
                    await stepContext.Context.SendActivityAsync("¿En qué más te puedo ayudar?. Si terminaste puedes escribir calificar para darme una puntuacion.", cancellationToken: cancellationToken);
                    return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
                }
            }
            await stepContext.Context.SendActivityAsync("Lo siento, no he podido encontrar tu numero de carnet en mi base de datos:", cancellationToken: cancellationToken);
            await stepContext.Context.SendActivityAsync("¿En que más te puedo ayudar?", cancellationToken: cancellationToken);
            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        }

        private bool isNumber(string message)
        {
            bool isNumeric = int.TryParse(message, out int n);
            return isNumeric;
        }
    }
}
