using GiftSmrBot.Core.DataInterfaces;
using GiftSmrBot.Core.Enums;
using GiftSmrBot.Core.Exceptions;
using GiftSmrBot.Core.Interfaces;
using System;
using System.Linq;

namespace GiftSmrBot.Services
{
    public class GetGiftState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IGiftService _giftService;

        public GetGiftState(IStateMachine stateMachine, IGiftService giftService)
        {
            _stateMachine = stateMachine;
            _giftService = giftService;
        }

        public MessageEventResult HandleEvent(MessageEvent messageEvent)
        {
            if (!int.TryParse(messageEvent.Message, out int price) || !Enum.IsDefined(typeof(PriceCategories), price))
                throw new PreviousStateUserAnswerException($"Некорректное значение ценовой категории {messageEvent.Message}");

            _stateMachine.GiftCriteria.AddPriceCategory((PriceCategories)price);

            var gifts = _giftService.GetGiftsByCriteria(_stateMachine.GiftCriteria).ToList();

            return new MessageEventResult(gifts.Count == 0 ? "Не удалось подобрать подарок по вашим критериям :(" : $"Подходящие подарки!\r\n{string.Join("\r\n", gifts)}");
        }
    }
}
