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
    public class TallerCards
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
        }

        private static Activity CreateCarousel()
        {
            var cardTaller1 = new HeroCard
            {
                Title = "Curso de Microsoft Teams y OneDrive",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://posbotstorage.blob.core.windows.net/images/Curso.PNG") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Más información", Value = "https://drive.google.com/file/d/1ho7299RvbIDLGctgNNThgwQfr5n3bu_D/view?usp=sharing", Type = ActionTypes.DownloadFile},
                    new CardAction(){Title = "Preinscripción", Value = "Preinscripcion~1", DisplayText = "Preinscripcion",  Type = ActionTypes.PostBack},
                }
            };

            var optionsAttachments = new List<Attachment>()
            {
                cardTaller1.ToAttachment(),
            };

            var reply = MessageFactory.Attachment(optionsAttachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            return reply as Activity;
        }
    }
}
