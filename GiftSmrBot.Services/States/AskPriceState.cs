using GiftSmrBot.Core.Enums;
using GiftSmrBot.Core.Exceptions;
using GiftSmrBot.Core.Interfaces;
using GiftSmrBot.Services.Helpers;
using System;

namespace GiftSmrBot.Services
{
    public class AskPriceState : IState
    {
        private readonly IStateMachine _stateMachine;

        public AskPriceState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public MessageEventResult HandleEvent(MessageEvent messageEvent)
        {
            if (!int.TryParse(messageEvent.Message, out int age) || !Enum.IsDefined(typeof(AgeCategories), age))
                throw new PreviousStateUserAnswerException($"Некорректное значение возраста {messageEvent.Message}");

            _stateMachine.GiftCriteria.AddAgeCategory((AgeCategories)age);

            var inlineKeyboard = KeyboardGenerator.GetFromEnum<PriceCategories>();

            return new MessageEventResult(inlineKeyboard, "Выберите ценовую категорию подарка");
        }
    }
}
