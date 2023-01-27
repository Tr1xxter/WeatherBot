using Telegram.Bot.Types.ReplyMarkups;

namespace WeatherBot.Domain.Telegram.Helpers
{
    public static class KeyboardMarkupHelper
    {
        public static ReplyKeyboardRemove Remove => new();

        public static ReplyKeyboardMarkup RequestLocation =>
            new(KeyboardButton.WithRequestLocation("Поделиться локацией"))
            {
                ResizeKeyboard = true
            };
    }
}
