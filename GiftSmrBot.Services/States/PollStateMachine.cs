using GiftSmrBot.Core;
using GiftSmrBot.Core.Exceptions;
using GiftSmrBot.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace GiftSmrBot.Services
{
    public class PollStateMachine : IStateMachine, IDisposable
    {
        private readonly Dictionary<string, IState> _stateStorage;
        private IStateFactory _stateFactory;
        private readonly IServiceScope _scope;
        private readonly ILogger<PollStateMachine> _logger;

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

        public PollStateMachine(IServiceProvider scopeFactory, ILogger<PollStateMachine> logger)
        {
            _stateStorage = new Dictionary<string, IState>();
            _scope = scopeFactory.CreateScope();
            _logger = logger;
            GiftCriteria = new GiftCriteriaBuilder();
        }

        public void SetState(string id, IState state)
        {
            _stateStorage[id] = state;
        }

        public MessageEventResult ProcessMessage(MessageEvent messageEvent)
        {
            MessageEventResult result = null;
            try
            {
                IStateFactory stateFactory = StateFactory;
                if (!_stateStorage.TryGetValue(messageEvent.Id, out IState currentState))
                {
                    currentState = stateFactory.GetInitialState();
                    SetState(messageEvent.Id, currentState);
                }                
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
                    _logger.LogInformation(prevStateAnswerEx, "Error in state processing " + prevStateAnswerEx.Message);
                    _logger.LogError(prevStateAnswerEx, prevStateAnswerEx.Message);
                    result = new MessageEventResult(prevStateAnswerEx.Message);
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return result ?? new MessageEventResult(string.Empty);
        }

        public bool FinalStateReached(string id) => _stateStorage.TryGetValue(id, out IState state) && state is GetGiftState;

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}
