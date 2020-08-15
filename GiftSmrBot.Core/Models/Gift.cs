using GiftSmrBot.Core.Enums;
using GiftSmrBot.Core.Extensions;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GiftSmrBot.Core.Models
{
    public class Gift
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false), Display(Name = "Название")]
        [DataType(DataType.Text)]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false), Display(Name = "Ссылка")]
        [DataType(DataType.Url)]
        public string Url { get; set; }
        [Required, Display(Name = "Стоимость, руб")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Required, Display(Name = "Возрастная категория")]
        public AgeCategories AgeCategory { get; set; }
        [Required, Display(Name = "Одаряемый")]
        public Recipients Recipient { get; set; }

        public Gift()
        {

        }

        public override string ToString()
        {
            return $@"
{Title}
{Price} рублей
{Url}
{AgeCategory.GetDisplayName()}
{Recipient.GetDisplayName()}";
        }
    }
}
