namespace GiftSmrBot.Core.Interfaces
{
    public interface IStateMachine
    {
        MessageEventResult ProcessMessage(MessageEvent messageEvent);
        void SetState(string id, IState state);
        bool FinalStateReached(string id);
        GiftCriteriaBuilder GiftCriteria { get; set; }
    }
}
