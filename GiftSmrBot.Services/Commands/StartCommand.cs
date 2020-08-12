using GiftSmrBot.Core.Interfaces;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GiftSmrBot.Services.Commands
{
    public class StartCommand : Command
    {
        private readonly IStateMachine _stateMachine;
        public override string Name => @"/start";

        public StartCommand(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override async Task ExecuteAsync(Message message, ITelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            var result = _stateMachine.ProcessMessage(new MessageEvent { Id = chatId.ToString(), Message = message.Text });
            await botClient.SendTextMessageAsync(
                chatId,
                result.Text,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                replyMarkup: result.ReplyMarkup
            );
        }
    }
}
