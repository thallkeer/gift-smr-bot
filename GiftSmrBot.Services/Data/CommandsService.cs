using GiftSmrBot.Core.DataInterfaces;
using GiftSmrBot.Core.Interfaces;
using GiftSmrBot.Services.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GiftSmrBot.Services.Data
{
    public class CommandsService : ICommandService
    {
        private readonly IStateMachine _stateMachine;
        private readonly ITelegramBotClient _botClient;
        private readonly IEnumerable<ICommand> _commands;
        private readonly IUserService _userService;

        public CommandsService(IStateMachine stateMachine, ITelegramBotClient botClient, IEnumerable<ICommand> commands, IUserService userService)
        {
            _stateMachine = stateMachine;
            _botClient = botClient;
            _commands = commands;
            _userService = userService;
        }

        public async Task TryExecuteCommandFromMessage(Message message)
        {
            var command = _commands.FirstOrDefault(c => c.Contains(message));
            if (command != null)
            {
                if (command.IsSecretCommand)
                {
                    var chatId = message.Chat.Id;
                    var user = await _userService.GetById(chatId);
                    if (user == null)
                    {
                        var loginCommand = _commands.OfType<LoginCommand>().First();
                        await _botClient.SendTextMessageAsync(chatId, $"Данная команда доступна только авторизованным пользователям. Воспользуйтесь командой {loginCommand.Name} 'пароль'.");
                        return;
                    }
                }
                await command.ExecuteAsync(message, _botClient);
            }
        }
        public async Task HandleCallbackQuery(CallbackQuery callbackQuery)
        {
            string chatId = callbackQuery.Message.Chat.Id.ToString();
            MessageEventResult result = _stateMachine.ProcessMessage(new MessageEvent { Id = chatId, Message = callbackQuery.Data });
            await _botClient.SendTextMessageAsync(
                chatId,
                result.Text,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                replyMarkup: result.ReplyMarkup
            );
        }
    }
}
