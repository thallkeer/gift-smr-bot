using GiftSmrBot.Core.DataInterfaces;
using GiftSmrBot.Core.Interfaces;
using System;

namespace GiftSmrBot.Services
{
    public class PollStateFactory : IStateFactory
    {
        private readonly IGiftService _giftService;
        private readonly IStateMachine _stateMachine;

        public PollStateFactory(IStateMachine stateMachine, IGiftService giftService)
        {
            _giftService = giftService;
            _stateMachine = stateMachine;
        }       

        public IState GetInitialState()
        {
            return new AskRecipientState(_stateMachine);
        }

        public IState GetNextState(IState prevState)
        {
            return prevState switch
            {
                //if prev state is null, return start state
                null => GetInitialState(),
                AskRecipientState _ => new AskAgeState(_stateMachine),
                AskAgeState _ => new AskPriceState(_stateMachine),
                AskPriceState _ => new GetGiftState(_stateMachine, _giftService),
                GetGiftState getGiftState => getGiftState,
                _ => throw new NotImplementedException($"Unexpected state type {prevState.GetType()} of previous state {prevState}"),
            };
        }

        public void Dispose()
        {
            _giftService?.Dispose();
        }
    }
}
