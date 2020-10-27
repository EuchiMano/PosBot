using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using PosgradoBot.Common.Model.Tickets;
using PosgradoBot.Common.Model.User;
using PosgradoBot.Data;
using PosgradoBot.Infrastructure.SendGridEmail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PosgradoBot.Dialogs.PersonalAtention
{
    public class AgentDialog : ComponentDialog
    {

        private IDataBaseService _databaseService;
        private readonly ISendGridEmailService _sendGridEmailService;
 
        public AgentDialog(IDataBaseService databaseService, ISendGridEmailService sendGridEmailService)
        {
            _sendGridEmailService = sendGridEmailService;
            _databaseService = databaseService;
            var waterfallSteps = new WaterfallStep[]
            {
                ToShowButton,
                ValidateOption,
                FinalProcess,
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
        }

        private async Task<DialogTurnResult> FinalProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var description = stepContext.Context.Activity.Text;
            

            //Save Email
            var name = stepContext.Context.Activity.From.Name;
            await stepContext.Context.SendActivityAsync("Seras atendido por un agente en cualquier momento");
            await SaveTicket(stepContext, description);
            await SendEmail(name, description);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
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
            var reply = MessageFactory.Text("Enseguida te atendera un agente: Abandonare la conversacion. ¿Estas de acuerdo con eso?");
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

        private async Task<DialogTurnResult> ValidateOption(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = stepContext.Context.Activity.Text;
            if (options == "Si")
            {
                return await stepContext.PromptAsync(
                            nameof(TextPrompt),
                            new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa una descripcion de tu consulta:") },
                            cancellationToken
                );
            }
            await stepContext.Context.SendActivityAsync("Vale, vuelve a preguntar cuando gustes, estoy para ayudarte.", cancellationToken: cancellationToken);
            await stepContext.Context.SendActivityAsync("¿En qué más quieres que te ayude?", cancellationToken: cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task SendEmail(string name, string description)
        {
            var time = DateTime.Now;
            string contentEmail = $"<h1>Ticket</h1>" +
                $"<p style='font-size:120 %;'>Hola Encargado, se creo un ticket con la siguiente información:</p>" +
                $"<p style='font-size:120 %;'><b>Nombre del usuario:</b> {name}</p>" +
                $"<p style='font-size:120 %;'><b>Fecha:</b> {time.ToShortDateString()}</p>" +
                $"<p style='font-size:120 %;'><b>Hora:</b> {time.ToShortTimeString()}</p>" +
                $"<p style='font-size:120 %;'><b>Descripción:</b> {description}</p>";

            await _sendGridEmailService.Execute(
                "marceps@outlook.es",
                "Chatbot Posgrado Service",
                "marce.ps666@gmail.com",
                "Encargado",
                "Ticket Atención Personal",
                "",
                contentEmail
                );
        }

        private async Task SaveTicket(WaterfallStepContext stepContext, string description)
        {
            var id = stepContext.Context.Activity.From.Id;
            var userlast = await _databaseService.User.FirstOrDefaultAsync(x => x.idChannel == id);
            var ticketModel = new TicketModel();
            ticketModel.idUser = userlast.id;
            ticketModel.registerDate = DateTime.Now;
            ticketModel.description = description;

            _databaseService.Ticket.Add(ticketModel);
            _databaseService.SaveChangesSQL();
        }
    }
}
