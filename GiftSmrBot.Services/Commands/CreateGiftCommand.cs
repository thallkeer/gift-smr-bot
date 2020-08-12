using GiftSmrBot.Core.DataInterfaces;
using GiftSmrBot.Core.Enums;
using GiftSmrBot.Core.Extensions;
using GiftSmrBot.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GiftSmrBot.Services.Commands
{
    public class CreateGiftCommand : Command
    {
        private readonly IGiftService _giftService;
        public override string Name => "/createGift";
        public override bool IsSecretCommand => true;

        public CreateGiftCommand(IGiftService giftService)
        {
            _giftService = giftService;
        }

        public override async Task ExecuteAsync(Message message, ITelegramBotClient client)
        {
            string messageText = message.Text.Trim();

            if (messageText == Name)
            {
                await client.SendTextMessageAsync(message.Chat.Id, $@"
Чтобы добавить подарок введите название команды и информацию о подарке, разделяя пункты запятыми :
1. Название
2. Ссылка
3. Стоимость
4. Возрастная категория
{EnumExtensions.DescribeEnum<AgeCategories>()}
5. Одаряемый
{EnumExtensions.DescribeEnum<Recipients>()}");
            }
            else
            {
                string[] args = messageText.Split(Name, StringSplitOptions.RemoveEmptyEntries)[0].Split(',');
                string messageForUser;
                if (ValidateGiftArgs(args))
                {
                    Gift gift = new Gift
                    {
                        Title = args[0].Trim(),
                        Url = args[1].Trim(),                        
                        AgeCategory = (AgeCategories)Enum.Parse(typeof(AgeCategories), args[3]),
                        Recipient = (Recipients)Enum.Parse(typeof(Recipients), args[4])
                    };
                    gift.SetPriceFromRubles(Convert.ToInt32(args[2]));
                    await _giftService.Create(gift);
                    messageForUser = "Подарок успешно создан!";
                }
                else
                {
                    messageForUser = "Не удалось создать подарок из переданных аргументов, проверьте правильность аргументов и попробуйте снова!";
                }
                await client.SendTextMessageAsync(message.Chat, messageForUser);
            }            
        }

        private bool ValidateGiftArgs(string[] args)
        {
            return args.Length == 5
                && (!string.IsNullOrEmpty(args[0]) && !string.IsNullOrEmpty(args[1]))
                && int.TryParse(args[2], out int _)
                && Enum.IsDefined(typeof(AgeCategories), Convert.ToInt32(args[3]))
                && Enum.IsDefined(typeof(Recipients), Convert.ToInt32(args[4]));
        }
    }
}
