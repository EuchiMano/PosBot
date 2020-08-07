using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PosgradoBot.Common.Cards
{
    public class DiplomadoCards
    {
        public static async Task<DialogTurnResult> ToShow(DialogContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = CreateCarousel()
                },
                cancellationToken
            );
            //await stepContext.Context.SendActivityAsync(activity: CreateCarousel(), cancellationToken);
        }

        private static Activity CreateCarousel()
        {
            var cardDiplomadoDisponible1 = new HeroCard
            {
                Title = "Diplomado en Educación Superior Universitaria a Distancia",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://posbotstorage.blob.core.windows.net/images/Curso.PNG") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Más información", Value = "https://posbotstorage.blob.core.windows.net/cursos/DESD-10.pdf", Type = ActionTypes.DownloadFile},
                    new CardAction(){Title = "Preinscripción", Value = "Preinscripcion~21", Type = ActionTypes.PostBack},
                }
            };

            var cardDiplomadoDisponible2 = new HeroCard
            {
                Title = "Diplomado en Planificación y Desarrollo de Competencias Profesionales en Educación Superior",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://posbotstorage.blob.core.windows.net/images/DiplomadoEduSup.PNG") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Más información", Value = "https://posbotstorage.blob.core.windows.net/cursos/DPDCV-3_oficial.pdf", Type = ActionTypes.DownloadFile},
                    new CardAction(){Title = "Preinscripción", Value = "Preinscripcion~22", Type = ActionTypes.PostBack},
                }
            };

            var cardDiplomadoDisponible3 = new HeroCard
            {
                Title = "Diplomado en Seguridad y Salud en el Trabajo",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://posbotstorage.blob.core.windows.net/images/DiplomadoEduSup.PNG") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Más información", Value = "https://posbotstorage.blob.core.windows.net/cursos/DSST-3_ oficial_.pdf", Type = ActionTypes.DownloadFile},
                    new CardAction(){Title = "Preinscripción", Value = "Preinscripcion~23", Type = ActionTypes.PostBack},
                }
            };

            var optionsAttachments = new List<Attachment>()
            {
                cardDiplomadoDisponible1.ToAttachment(),
                cardDiplomadoDisponible2.ToAttachment(),
                cardDiplomadoDisponible3.ToAttachment(),
            };

            var reply = MessageFactory.Attachment(optionsAttachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            return reply as Activity;
        }
    }
}
