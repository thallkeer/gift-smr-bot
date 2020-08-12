using Telegram.Bot.Types.ReplyMarkups;

namespace GiftSmrBot.Core.Interfaces
{
    public class MessageEventResult
    {
        public string Text { get; set; }
        public IReplyMarkup ReplyMarkup { get; set; }

        public MessageEventResult(IReplyMarkup replyMarkup, string text) : this(text)
        {
            ReplyMarkup = replyMarkup;
        }

        public MessageEventResult(string text)
        {
            Text = text;
        }
    }
}