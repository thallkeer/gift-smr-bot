using GiftSmrBot.Core;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace GiftSmrBot.Services
{
    public class BotService : IBotService
    {
        private readonly BotSettings _config;
        private readonly TelegramBotClient _botClient;

        public ITelegramBotClient BotClient => _botClient;

        public BotService(IOptions<BotSettings> config)
        {
            _config = config.Value;
            _botClient = new TelegramBotClient(_config.Token);
            string hook = $@"{_config.Host}/api/update";
            _botClient.SetWebhookAsync(hook).Wait();
        }       
    }
}
