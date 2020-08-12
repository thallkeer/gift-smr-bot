using System;

namespace GiftSmrBot.Core.Interfaces
{
    public interface IStateFactory : IDisposable
    {
        IState GetInitialState();
        IState GetNextState(IState prevState);
    }
}
