using GiftSmrBot.Core.DataInterfaces;
using GiftSmrBot.Core.Enums;
using GiftSmrBot.Core.Extensions;
using GiftSmrBot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                if (ParsedGiftArgs.ValidateAndReturnParsed(args, out ParsedGiftArgs parsedGiftArgs))
                {
                    List<Gift> gifts = new List<Gift>(parsedGiftArgs.Recipients.Count);
                    foreach (int recipient in parsedGiftArgs.Recipients)
                    {
                        Gift gift = new Gift
                        {
                            Title = parsedGiftArgs.Title,
                            Url = parsedGiftArgs.Url,
                            Price = parsedGiftArgs.Price,
                            AgeCategory = (AgeCategories)parsedGiftArgs.AgeCategory,
                            Recipient = (Recipients)recipient
                        };
                        gifts.Add(gift);
                    }
                    await _giftService.CreateRangeAsync(gifts);
                    messageForUser = "Подарок успешно создан!";
                }
                else
                {
                    messageForUser = "Не удалось создать подарок из переданных аргументов, проверьте правильность аргументов и попробуйте снова!";
                }
                await client.SendTextMessageAsync(message.Chat, messageForUser);
            }            
        }

        private class ParsedGiftArgs
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public double Price { get; set; }
            public int AgeCategory { get; set; }
            public List<int> Recipients { get; set; }

            private ParsedGiftArgs() { }

            public static bool ValidateAndReturnParsed(string[] args, out ParsedGiftArgs parsedGiftArgs)
            {
                parsedGiftArgs = null;
                if (args.Length == 5)
                    if (!string.IsNullOrEmpty(args[0]) && !string.IsNullOrEmpty(args[1]))
                        if (double.TryParse(args[2], out double price) && int.TryParse(args[3], out int ageCategory))

                            if (Enum.IsDefined(typeof(AgeCategories), ageCategory))
                            {
                                var recipientArgs = args[4].Split(';').ToList();
                                List<int> recipients = new List<int>(recipientArgs.Count);
                                recipientArgs.ForEach(arg =>
                                {
                                    if (int.TryParse(arg, out int recipient))
                                        recipients.Add(recipient);
                                });
                                if (recipients.Count == recipientArgs.Count)
                                {
                                    parsedGiftArgs = new ParsedGiftArgs
                                    {
                                        Title = args[0].Trim(),
                                        Url = args[1].Trim(),
                                        Price = price,
                                        AgeCategory = ageCategory,
                                        Recipients = recipients
                                    };
                                    return true;
                                }
                            }
                return false;
            }
        }
    }
}
