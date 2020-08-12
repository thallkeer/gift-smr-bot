using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GiftSmrBot.Core.Interfaces
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        bool IsSecretCommand { get; }
        Task ExecuteAsync(Message message, ITelegramBotClient client);
        bool Contains(Message message);
    }
}
