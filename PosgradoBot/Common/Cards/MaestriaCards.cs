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
                Images = new List<CardImage> { new CardImage("https://posbot1storage.blob.core.windows.net/cursos/MAE-3_%20oficial.pdf") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Más información", Value = "https://posbotstorage.blob.core.windows.net/cursos/MAE-3_ oficial.pdf", Type = ActionTypes.DownloadFile},
                    new CardAction(){Title = "Preinscripción", Value = "Preinscripcion~31", Type = ActionTypes.PostBack},
                }
            };

            var cardMaestriasDisponible2 = new HeroCard
            {
                Title = "Maestría en Recursos Naturales y Gestión Ambiental",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://posbot1storage.blob.core.windows.net/cursos/MAGA-4_oficial.pdf") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Más información", Value = "https://posbotstorage.blob.core.windows.net/cursos/MAGA-4_oficial.pdf", Type = ActionTypes.DownloadFile},
                    new CardAction(){Title = "Preinscripción", Value = "Preinscripcion~32", Type = ActionTypes.PostBack},
                }
            };

            var cardMaestriasDisponible3 = new HeroCard
            {
                Title = "Maestría en Educación Superior Universitaria",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://posbot1storage.blob.core.windows.net/cursos/MESU-10_oficial.pdf") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Más información", Value = "https://posbotstorage.blob.core.windows.net/cursos/MESU-10_oficial.pdf", Type = ActionTypes.DownloadFile},
                    new CardAction(){Title = "Preinscripción", Value = "Preinscripcion~33", Type = ActionTypes.PostBack},
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
