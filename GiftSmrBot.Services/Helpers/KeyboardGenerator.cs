using GiftSmrBot.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Telegram.Bot.Types.ReplyMarkups;

namespace GiftSmrBot.Services.Helpers
{
    public class KeyboardGenerator
    {
        public static InlineKeyboardMarkup GetFromEnum<T>() where T : Enum
        {
            var ageCategories = Enum.GetValues(typeof(T));

            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>(ageCategories.Length);

            foreach (T category in ageCategories)
            {
                var enumDescription = category.DescribeEnum();
                buttons.Add(InlineKeyboardButton.WithCallbackData(enumDescription.displayName, enumDescription.value.ToString()));
            }

            return new InlineKeyboardMarkup(new[] { buttons });
        }
    }
}
