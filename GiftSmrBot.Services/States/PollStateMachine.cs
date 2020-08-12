using GiftSmrBot.Core;
using GiftSmrBot.Core.Exceptions;
using GiftSmrBot.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace GiftSmrBot.Services
{
    public class PollStateMachine : IStateMachine, IDisposable
    {
        private readonly Dictionary<string, IState> _stateStorage;
        private IStateFactory _stateFactory;
        private readonly IServiceScope _scope;
        public GiftCriteriaBuilder GiftCriteria { get; set; }
        private IStateFactory StateFactory
        {
            get
            {
                using (_stateFactory = _scope.ServiceProvider.GetRequiredService<IStateFactory>())
                {
                    return _stateFactory;
                }
            }
        }

        public PollStateMachine(IServiceProvider scopeFactory)
        {
            _stateStorage = new Dictionary<string, IState>();
            _scope = scopeFactory.CreateScope();
            GiftCriteria = new GiftCriteriaBuilder();
        }

        public void SetState(string id, IState state)
        {
            _stateStorage[id] = state;
        }

        public MessageEventResult ProcessMessage(MessageEvent messageEvent)
        {
            IStateFactory stateFactory = StateFactory;
            if (!_stateStorage.TryGetValue(messageEvent.Id, out IState currentState))
            {
                currentState = stateFactory.GetInitialState();
                SetState(messageEvent.Id, currentState);
            }
            MessageEventResult result;
            try
            {
                result = currentState.HandleEvent(messageEvent);
                if (!FinalStateReached(messageEvent.Id))
                {
                    IState nextState = stateFactory.GetNextState(currentState);
                    SetState(messageEvent.Id, nextState);
                }
                else
                    SetState(messageEvent.Id, stateFactory.GetInitialState());
            }
            catch (PreviousStateUserAnswerException prevStateAnswerEx)
            {
                result = new MessageEventResult(prevStateAnswerEx.Message);
            }           
            return result;
        }

        public bool FinalStateReached(string id) => _stateStorage.TryGetValue(id, out IState state) && state is GetGiftState;

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}
