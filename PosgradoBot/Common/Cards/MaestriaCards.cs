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
    public class MaestriaCards
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
            var cardMaestriasDisponible1 = new HeroCard
            {
                Title = "Maestría en Administración de Empresas",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://posbotstorage.blob.core.windows.net/images/Curso.PNG") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Más información", Value = "https://drive.google.com/file/d/1VGCuy9vnokS-F9sNcLtgNMWefYRcQEgS/view?usp=sharing", Type = ActionTypes.DownloadFile},
                    new CardAction(){Title = "Preinscripción", Value = "Preinscripcion~4", Type = ActionTypes.PostBack},
                }
            };

            var cardMaestriasDisponible2 = new HeroCard
            {
                Title = "Maestría en Recursos Naturales y Gestión Ambiental",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://posbotstorage.blob.core.windows.net/images/DiplomadoEduSup.PNG") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Más información", Value = "https://drive.google.com/file/d/177QzKOAEZ_Qk8rKb6_ILr0HkmXC4KKfz/view?usp=sharing", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "Preinscripción", Value = "Preinscripcion~5", Type = ActionTypes.PostBack},
                }
            };

            var cardMaestriasDisponible3 = new HeroCard
            {
                Title = "Maestría en Educación Superior Universitaria",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://posbotstorage.blob.core.windows.net/images/DiplomadoEduSup.PNG") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Más información", Value = "https://drive.google.com/file/d/1W6CCWM2kR_YSwCc4J_nfwv87kCCmVrTw/view?usp=sharing", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "Preinscripción", Value = "Preinscripcion~6", Type = ActionTypes.PostBack},
                }
            };

            var optionsAttachments = new List<Attachment>()
            {
                cardMaestriasDisponible1.ToAttachment(),
                cardMaestriasDisponible2.ToAttachment(),
                cardMaestriasDisponible3.ToAttachment(),
            };

            var reply = MessageFactory.Attachment(optionsAttachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            return reply as Activity;
        }
    }
}
