using GiftSmrBot.Core.Enums;
using GiftSmrBot.Core.Exceptions;
using GiftSmrBot.Core.Interfaces;
using GiftSmrBot.Services.Helpers;
using System;

namespace GiftSmrBot.Services
{
    public class AskAgeState : IState
    {
        private readonly IStateMachine _stateMachine;

        public AskAgeState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public MessageEventResult HandleEvent(MessageEvent messageEvent)
        {
            if (!int.TryParse(messageEvent.Message, out int recipient) || !Enum.IsDefined(typeof(Recipients), recipient))
                throw new PreviousStateUserAnswerException($"Некорректное значение одаряемого {messageEvent.Message}");

            _stateMachine.GiftCriteria.AddRecipient((Recipients)recipient);

            var inlineKeyboard = KeyboardGenerator.GetFromEnum<AgeCategories>();

            return new MessageEventResult(inlineKeyboard, "Выберите возрастную категорию");
        }
    }
}
