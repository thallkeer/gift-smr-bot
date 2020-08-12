using GiftSmrBot.Core.Interfaces;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GiftSmrBot.Services.Commands
{
    public abstract class Command : ICommand
    {
        public abstract string Name { get; }
        public virtual string Description => string.Empty;
        public virtual bool IsSecretCommand => false;

        public abstract Task ExecuteAsync(Message message, ITelegramBotClient client);

        public virtual bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(this.Name);
        }
    }
}
