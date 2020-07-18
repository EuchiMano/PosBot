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
                    new CardAction(){Title = "Más información", Value = "https://drive.google.com/file/d/1rheUW7gdnNcCYXt1mUyNlhMyJaeTUwoc/view?usp=sharing", Type = ActionTypes.DownloadFile},
                    new CardAction(){Title = "Preinscripción", Value = "Preinscripcion~2", Type = ActionTypes.PostBack},
                }
            };

            var cardDiplomadoDisponible2 = new HeroCard
            {
                Title = "Diplomado en Planificación y Desarrollo de Competencias Profesionales en Educación Superior",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://posbotstorage.blob.core.windows.net/images/DiplomadoEduSup.PNG") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Más información", Value = "https://drive.google.com/file/d/17CemKcOd6gGBv-tZGsOx7yP3dbhBWboW/view?usp=sharing", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "Preinscripción", Value = "Preinscripcion~3", Type = ActionTypes.PostBack},
                }
            };

            var optionsAttachments = new List<Attachment>()
            {
                cardDiplomadoDisponible1.ToAttachment(),
                cardDiplomadoDisponible2.ToAttachment(),
            };

            var reply = MessageFactory.Attachment(optionsAttachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            return reply as Activity;
        }
    }
}
