using Telegram.Bot;

namespace GiftSmrBot.Core
{
    public interface IBotService
    {
        ITelegramBotClient BotClient { get; }        
    }
}
