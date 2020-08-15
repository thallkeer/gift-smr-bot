using GiftSmrBot.Core.DataInterfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GiftSmrBot.Services.Commands
{
    public class LoginCommand : Command
    {
        private readonly string _secretPassword;
        private readonly IUserService _userService;

        public override string Name => @"/login";
        public override bool IsSecretCommand => false;

        public LoginCommand(IOptions<SecretLogin> loginOptions, IUserService userService)
        {
            _secretPassword = loginOptions.Value.Password;
            _userService = userService;
        }

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(this.Name);
        }

        public override async Task ExecuteAsync(Message message, ITelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            var args = message.Text.Trim().Split(" ");
            
            if (args.Length == 2 && args[1] == _secretPassword)
            {
                await _userService.CreateAsync(new Core.Models.User(message.Chat.Id));
                await botClient.SendTextMessageAsync(chatId, "Вы вошли в качестве администратора.");
            }
            else
                await botClient.SendTextMessageAsync(chatId, "Неверный пароль!");
        }
    }
}
