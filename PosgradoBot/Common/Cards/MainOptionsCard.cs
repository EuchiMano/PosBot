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
    public class MainOptionsCard
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
            var cardCursosDisponibles = new HeroCard
            {
                Title = "Educación Superior Universitaria",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://posbotstorage.blob.core.windows.net/images/Curso.PNG") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Más información", Value = "Mas informacion", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "Preinscripción", Value = "Preinscripcion", Type = ActionTypes.ImBack},
                }
            };

            var cardInformacionContacto = new HeroCard
            {
                Title = "Información de Contacto",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://posbotstorage.blob.core.windows.net/images/DiplomadoEduSup.PNG") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Centro de Contacto", Value = "Centro de Contacto", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "Sitio web", Value = "https://docs.microsoft.com/", Type = ActionTypes.OpenUrl},
                }
            };

            var cardCalificacion = new HeroCard
            {
                Title = "Calificación",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://posbotstorage.blob.core.windows.net/images/DiplomadoEduSup.PNG") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Calificar Bot", Value = "Calificar Bot", Type = ActionTypes.ImBack}
                    
                }
            };

            var optionsAttachments = new List<Attachment>()
            {
                cardCursosDisponibles.ToAttachment(),
                cardInformacionContacto.ToAttachment(),
                cardCalificacion.ToAttachment(),
            };

            var reply = MessageFactory.Attachment(optionsAttachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            return reply as Activity;
        }
    }
}
