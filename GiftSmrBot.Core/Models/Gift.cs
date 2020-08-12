using GiftSmrBot.Core.Enums;
using GiftSmrBot.Core.Extensions;
using Newtonsoft.Json;

namespace GiftSmrBot.Core.Models
{
    public class Gift
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Price { get; set; }
        public AgeCategories AgeCategory { get; set; }
        public Recipients Recipient { get; set; }
        [JsonIgnore]
        public double PriceToRubles => Price / 100;

        public Gift()
        {

        }

        public void SetPriceFromRubles(double rubles)
        {
            Price = (int)rubles * 100;
        }

        public override string ToString()
        {
            return $@"
{Title}
{PriceToRubles} рублей
{Url}
{AgeCategory.GetDisplayName()}
{Recipient.GetDisplayName()}";
        }
    }
}
