namespace GiftSmrBot.Core.Interfaces
{
    public interface IState
    {
        MessageEventResult HandleEvent(MessageEvent messageEvent);
    }
}
