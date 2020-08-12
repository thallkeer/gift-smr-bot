using System;

namespace GiftSmrBot.Core.Exceptions
{
    public class PreviousStateUserAnswerException : Exception
    {
        public PreviousStateUserAnswerException(string message) : base(message)
        {

        }
    }
}
