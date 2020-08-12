using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace GiftSmrBot.Core.DataInterfaces
{
    public interface ICommandService
    {
        Task TryExecuteCommandFromMessage(Message message);
        Task HandleCallbackQuery(CallbackQuery callbackQuery);
    }
}
