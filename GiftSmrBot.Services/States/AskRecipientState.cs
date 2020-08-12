using GiftSmrBot.Core.Enums;
using GiftSmrBot.Core.Interfaces;
using GiftSmrBot.Services.Helpers;

namespace GiftSmrBot.Services
{
    public class AskRecipientState : IState
    {
        private readonly IStateMachine _stateMachine;

        public AskRecipientState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public MessageEventResult HandleEvent(MessageEvent messageEvent)
        {
            var inlineKeyboard = KeyboardGenerator.GetFromEnum<Recipients>();

            return new MessageEventResult(inlineKeyboard, "Кому вы выбираете подарок?");
        }
    }
}
