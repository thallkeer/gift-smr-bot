using GiftSmrBot.Core.DataInterfaces;
using GiftSmrBot.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GiftSmrBot.Services.Commands
{
    public class ShowGiftsCommand : Command
    {
        private readonly IGiftService _giftService;
        public override string Name => "/showGifts";
        public override bool IsSecretCommand => true;

        public ShowGiftsCommand(IGiftService giftService)
        {
            _giftService = giftService;
        }

        public override async Task ExecuteAsync(Message message, ITelegramBotClient client)
        {
            var gifts = await _giftService.GetAll().ToListAsync();
            string giftsToString = gifts.Count != 0 ? string.Join('\n', gifts) : "В базе пока еще нет добавленных подарков!";
            await client.SendTextMessageAsync(message.Chat, giftsToString);
        }
    }
}
