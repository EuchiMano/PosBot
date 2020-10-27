using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using PosgradoBot.Common.Model.BotState;
using PosgradoBot.Common.Model.Preinscription;
using PosgradoBot.Common.Model.User;
using PosgradoBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PosgradoBot.Dialogs.Curses
{
    public class PreinscriptionDialog : ComponentDialog
    {
        private readonly IDataBaseService _databaseService;
        public static UserModel newUserModel = new UserModel();
        public static PreinscriptionModel preIncriptionModel = new PreinscriptionModel();

        private readonly IStatePropertyAccessor<BotStateModel> _userState;

        public PreinscriptionDialog(IDataBaseService databaseService, UserState userState)
        {
            _userState = userState.CreateProperty<BotStateModel>(nameof(BotStateModel));
            _databaseService = databaseService;


            var waterfallStep = new WaterfallStep[]
            {
                SetForm,
                SetFormVal,
                SetLastNameP,
                SetLastNameM,
                SetNames,
                SetCi,
                SetTelefono,
                Validation,
                SetType,
                SetGrado,
                SetFuerza,
                SetCodigo,
                SetCarrera,
                SetFechaEgreso,
                Final

            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallStep));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
        }

        private async Task<DialogTurnResult> SetFormVal(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (userStateModel.forms)
            {
                var formopt = stepContext.Context.Activity.Text;
                if (formopt == "Si")
                {
                    userStateModel.forms = false;
                }
            }
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> SetForm(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (userStateModel.forms)
            {
                return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions { Prompt = CreateButtonForm() },
                cancellationToken);
            }
            else
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
        }

        private async Task<DialogTurnResult> SetFechaEgreso(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (userStateModel.forms)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                if (preIncriptionModel.grado != null)
                {
                    return await stepContext.NextAsync(cancellationToken: cancellationToken);
                }

                if (preIncriptionModel.codigoexemi != null)
                {
                    var excarrera = stepContext.Context.Activity.Text;
                    preIncriptionModel.excarrera = excarrera;

                    return await stepContext.PromptAsync(
                        nameof(TextPrompt),
                        new PromptOptions { Prompt = MessageFactory.Text("Por favor escribe tu fecha de Egreso de la EMI Semestre/Año: ") },
                        cancellationToken
                    );
                }
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
        }

        private async Task<DialogTurnResult> Final(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());

            if (userStateModel.forms)
            {
                //Save Data Reusing Forms
                string userIdform = stepContext.Context.Activity.From.Id;

                var preUserform = await _databaseService.User.FirstOrDefaultAsync(x => x.idChannel == userIdform);
                var preInscriptionModelFirst = await _databaseService.Preinscription.FirstOrDefaultAsync(x => x.idUser == preUserform.id);

                var newInscriptionModelR = new PreinscriptionModel();
                newInscriptionModelR.idUser = preInscriptionModelFirst.idUser;
                newInscriptionModelR.idCurse = preIncriptionModel.idCurse;

                newInscriptionModelR.apellidoP = preInscriptionModelFirst.apellidoP;
                newInscriptionModelR.apellidoM = preInscriptionModelFirst.apellidoM;
                newInscriptionModelR.names = preInscriptionModelFirst.names;
                newInscriptionModelR.ci = preInscriptionModelFirst.ci;
                newInscriptionModelR.celular = preInscriptionModelFirst.celular;
                newInscriptionModelR.grado = preInscriptionModelFirst.grado;
                newInscriptionModelR.fuerza = preInscriptionModelFirst.fuerza;
                newInscriptionModelR.codigoexemi = preInscriptionModelFirst.codigoexemi;
                newInscriptionModelR.excarrera = preInscriptionModelFirst.excarrera;
                newInscriptionModelR.fechaegreso = preInscriptionModelFirst.fechaegreso;

                await _databaseService.Preinscription.AddAsync(newInscriptionModelR);
                await _databaseService.SaveAsync();


                await stepContext.Context.SendActivityAsync("Tu preinscripción se guardo con exito.", cancellationToken: cancellationToken);
                await stepContext.Context.SendActivityAsync("¿En qué más puedo ayudarte?", cancellationToken: cancellationToken);
                preIncriptionModel = new PreinscriptionModel();
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
                //Update User
                //userModel.lastName = newUserModel.lastName;
                //userModel.name = newUserModel.name;
                //userModel.ci = newUserModel.ci;
                //userModel.correo = newUserModel.correo;
                //userModel.celular = newUserModel.celular;

                //_databaseService.User.Update(userModel);
                //await _databaseService.SaveAsync();
            }

            if (preIncriptionModel.codigoexemi != null)
            {
                var fechaegre = stepContext.Context.Activity.Text;
                preIncriptionModel.fechaegreso = fechaegre;
            }

            string userId = stepContext.Context.Activity.From.Id;
            var preUser = await _databaseService.User.FirstOrDefaultAsync(x => x.idChannel == userId);

            //Save Preinscription
            var newInscriptionModel = new PreinscriptionModel();
            newInscriptionModel.idUser = preUser.id;
            newInscriptionModel.idCurse = preIncriptionModel.idCurse;
            newInscriptionModel.apellidoP = preIncriptionModel.apellidoP;
            newInscriptionModel.apellidoM = preIncriptionModel.apellidoM;
            newInscriptionModel.names = preIncriptionModel.names;
            newInscriptionModel.ci = preIncriptionModel.ci;
            newInscriptionModel.celular = preIncriptionModel.celular;
            newInscriptionModel.grado = preIncriptionModel.grado;
            newInscriptionModel.fuerza = preIncriptionModel.fuerza;
            newInscriptionModel.codigoexemi = preIncriptionModel.codigoexemi;
            newInscriptionModel.excarrera = preIncriptionModel.excarrera;
            newInscriptionModel.fechaegreso = preIncriptionModel.fechaegreso;

            await _databaseService.Preinscription.AddAsync(newInscriptionModel);
            await _databaseService.SaveAsync();

            await stepContext.Context.SendActivityAsync("Tu preinscripción se guardo con exito.", cancellationToken: cancellationToken);
            userStateModel.forms = true;

            //Show Summary
            //string summaryPreinscription = $"Apellidos: {preIncriptionModel.apellidoM}" +
            //    $"{Environment.NewLine} Telefono: {preIncriptionModel.celular}" +
            //    $"{Environment.NewLine} Correo: {preIncriptionModel.correo}" +
            //    $"{Environment.NewLine} Ci: {preIncriptionModel.ci}" +
            //    $"{Environment.NewLine} Curso: {preIncriptionModel.curso}" +
            //    $"{Environment.NewLine} Nombres: {preIncriptionModel.names}";

            //await stepContext.Context.SendActivityAsync(summaryPreinscription, cancellationToken: cancellationToken);
            //await Task.Delay(1000);
            await stepContext.Context.SendActivityAsync("¿En qué más puedo ayudarte?", cancellationToken: cancellationToken);
            preIncriptionModel = new PreinscriptionModel();
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);

        }

        private async Task<DialogTurnResult> SetCarrera(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (userStateModel.forms)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                if (preIncriptionModel.grado != null)
                {
                    return await stepContext.NextAsync(cancellationToken: cancellationToken);
                }

                if (preIncriptionModel.codigoexemi != null)
                {
                    var codemi = stepContext.Context.Activity.Text;
                    preIncriptionModel.codigoexemi = codemi;
                    return await stepContext.PromptAsync(
                        nameof(TextPrompt),
                        new PromptOptions { Prompt = MessageFactory.Text("Por favor escribe la carrera a la cual pertenecías de la EMI:") },
                        cancellationToken
                    );
                }
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
        }

        private async Task<DialogTurnResult> SetCodigo(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (userStateModel.forms)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                if (preIncriptionModel.grado != null)
                {
                    var fuerza = stepContext.Context.Activity.Text;
                    preIncriptionModel.fuerza = fuerza;
                    return await stepContext.NextAsync(cancellationToken: cancellationToken);
                }
                if (preIncriptionModel.codigoexemi != null)
                {
                    return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor escribe tu código de Ex-Alumno de la EMI:") },
                    cancellationToken
                );
                }
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
        }

        private async Task<DialogTurnResult> SetFuerza(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (userStateModel.forms)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                if (preIncriptionModel.grado != null)
                {
                    var grado = stepContext.Context.Activity.Text;
                    preIncriptionModel.grado = grado;

                    return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions { Prompt = MessageFactory.Text("Por favor escribe a que fuerza militar perteneces:") },
                cancellationToken
                            );
                }
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
        }

        private async Task<DialogTurnResult> SetGrado(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (userStateModel.forms)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                var option = stepContext.Context.Activity.Text;

                if (option == "PM")
                {
                    preIncriptionModel.grado = "null";
                    return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor escribe tu grado militar:") },
                    cancellationToken
                                );
                }
                if (option == "EMI")
                {
                    preIncriptionModel.codigoexemi = "null";
                }
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
        }

        private async Task<DialogTurnResult> SetType(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (userStateModel.forms)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                var userConfirmation = stepContext.Context.Activity.Text;

                if (userConfirmation.Equals("EMI"))
                {
                    preIncriptionModel.grado = null;
                    preIncriptionModel.fuerza = null;

                    return await stepContext.NextAsync(cancellationToken: cancellationToken);
                }
                else
                {
                    if (userConfirmation.Equals("PM"))
                    {
                        preIncriptionModel.codigoexemi = null;
                        preIncriptionModel.excarrera = null;
                        preIncriptionModel.fechaegreso = null;
                        return await stepContext.NextAsync(cancellationToken: cancellationToken);
                    }
                    else
                    {
                        preIncriptionModel.grado = null;
                        preIncriptionModel.fuerza = null;
                        preIncriptionModel.codigoexemi = null;
                        preIncriptionModel.excarrera = null;
                        preIncriptionModel.fechaegreso = null;
                        return await stepContext.NextAsync(cancellationToken: cancellationToken);
                    }
                }
            }
        }

        private async Task<DialogTurnResult> Validation(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (stepContext.Options is int stepToExecute && stepToExecute != stepContext.Index)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            if (userStateModel.forms)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                if (preIncriptionModel.celular != null)
                {
                    return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions { Prompt = CreateButtonType() },
                cancellationToken
                );
                }
                var celular = stepContext.Context.Activity.Text;
                var res = isNumber(celular);

                if (!res)
                {
                    await stepContext.Context.SendActivityAsync($"Hey, en este campo solo se permite numeros...", cancellationToken: cancellationToken);
                    return await stepContext.ReplaceDialogAsync(nameof(PreinscriptionDialog), options: 6);
                }

                preIncriptionModel.celular = celular;

                return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions { Prompt = CreateButtonType() },
                cancellationToken
            );
            }
        }

        private async Task<DialogTurnResult> SetTelefono(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (stepContext.Options is int stepToExecute && stepToExecute != stepContext.Index)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }

            if (userStateModel.forms)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                if (preIncriptionModel.ci != null)
                {
                    return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor escribe tu número de telefono o celular de contacto:") },
                    cancellationToken
                );
                }
                var ci = stepContext.Context.Activity.Text;
                var res = isNumber(ci);

                if (!res)
                {
                    await stepContext.Context.SendActivityAsync($"Hey, en este campo solo se permite numeros...", cancellationToken: cancellationToken);
                    return await stepContext.ReplaceDialogAsync(nameof(PreinscriptionDialog), options: 5);
                }

                preIncriptionModel.ci = ci;

                return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor escribe tu número de telefono o celular de contacto:") },
                    cancellationToken
                );
            }
        }

        private async Task<DialogTurnResult> SetLastNameM(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            //if(stepContext.Options is int stepToExecute && stepToExecute != stepContext.Index)
            //{
            //    return await stepContext.NextAsync();
            //}

            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (stepContext.Options is int stepToExecute && stepToExecute != stepContext.Index)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }

            if (userStateModel.forms)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                if (preIncriptionModel.apellidoP != null)
                {
                    return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa tu apellido materno:") },
                    cancellationToken
                );
                }
                var lastNameP = stepContext.Context.Activity.Text;
                var res = isNumber(lastNameP);

                if (res)
                {
                    await stepContext.Context.SendActivityAsync($"Hey, en este campo solo se permite texto...", cancellationToken: cancellationToken);
                    return await stepContext.ReplaceDialogAsync(nameof(PreinscriptionDialog), options: 2);
                }
                preIncriptionModel.apellidoP = lastNameP;

                return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa tu apellido materno:") },
                    cancellationToken
                );
            }
        }

        private async Task<DialogTurnResult> SetCi(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());
            if (stepContext.Options is int stepToExecute && stepToExecute != stepContext.Index)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            if (userStateModel.forms)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                if (preIncriptionModel.names != null)
                {
                    return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa tu número de carnet de identidad:") },
                    cancellationToken
                );
                }
                var fullName = stepContext.Context.Activity.Text;
                var res = isNumber(fullName);

                if (res)
                {
                    await stepContext.Context.SendActivityAsync($"Hey, en este campo solo se permite texto...", cancellationToken: cancellationToken);
                    return await stepContext.ReplaceDialogAsync(nameof(PreinscriptionDialog), options: 4);
                }

                preIncriptionModel.names = fullName;

                return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa tu número de carnet de identidad:") },
                    cancellationToken
                );
            }
        }

        private async Task<DialogTurnResult> SetNames(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());

            if (stepContext.Options is int stepToExecute && stepToExecute != stepContext.Index)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }

            if (userStateModel.forms)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                if (preIncriptionModel.apellidoM != null)
                {
                    return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa tu nombre completo:") },
                    cancellationToken
                );
                }
                var lastNameM = stepContext.Context.Activity.Text;
                var res = isNumber(lastNameM);

                if (res)
                {
                    await stepContext.Context.SendActivityAsync($"Hey, en este campo solo se permite texto...", cancellationToken: cancellationToken);
                    return await stepContext.ReplaceDialogAsync(nameof(PreinscriptionDialog), options: 3);
                }

                preIncriptionModel.apellidoM = lastNameM;

                return await stepContext.PromptAsync(
                    nameof(TextPrompt),
                    new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa tu nombre completo:") },
                    cancellationToken
                );
            }
        }

        private async Task<DialogTurnResult> SetLastNameP(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //string msgFromPreviousDialog = (string)stepContext.ActiveDialog.State["options"];
            //preIncriptionModel.idCurse = int.Parse(msgFromPreviousDialog);

            var userStateModel = await _userState.GetAsync(stepContext.Context, () => new BotStateModel());

            if (stepContext.Options is int stepToExecute && stepToExecute != stepContext.Index)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }

            if (userStateModel.forms)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
            else
            {
                return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions { Prompt = MessageFactory.Text("Por favor ingresa tu apellido paterno:") },
                cancellationToken
            );
            }
        }

        private Activity CreateButtonConfirmation()
        {
            var reply = MessageFactory.Text("¿Confirmas la creación de este formulario?");
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

        private Activity CreateButtonType()
        {
            var reply = MessageFactory.Text("¿Perteneces a alguno de estos grupos?");
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){Title = "Personal Militar", Value = "PM", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "Ex-Alumno EMI", Value = "EMI", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "Ninguno", Value = "NO", Type = ActionTypes.ImBack},
                }
            };
            return reply as Activity;
        }

        private Activity CreateButtonForm()
        {
            var reply = MessageFactory.Text("¿Quieres rellenar un nuevo formulario?");
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

        private bool isNumber(string message)
        {
            bool isNumeric = int.TryParse(message, out int n);
            return isNumeric;
        }
    }
}