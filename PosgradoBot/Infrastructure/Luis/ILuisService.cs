using Microsoft.Bot.Builder.AI.Luis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PosgradoBot.Infrastructure.Luis
{
    public interface ILuisService
    {
        public LuisRecognizer _luisRecognizer { get; set; }
    }
}
