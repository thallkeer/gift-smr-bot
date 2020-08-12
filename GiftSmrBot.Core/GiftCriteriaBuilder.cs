using GiftSmrBot.Core.Enums;

namespace GiftSmrBot.Core
{
    public class GiftCriteriaBuilder
    {
        //private AgeCategories _age { get; set; }
        //private PriceCategories _price { get; set; }
        //private Recipients _recipient { get; set; }
        //(int) _age;
        //(int) _price;
        //(int) _recipient;
        public AgeCategories Age { get; private set; }
        public PriceCategories Price { get; private set; }
        public Recipients Recipient { get; private set; }

        public GiftCriteriaBuilder()
        {

        }

        public GiftCriteriaBuilder AddAgeCategory(AgeCategories ageCategory)
        {
            Age = ageCategory;
            return this;
        }

        public GiftCriteriaBuilder AddPriceCategory(PriceCategories priceCategory)
        {
            Price = priceCategory;
            return this;
        }

        public GiftCriteriaBuilder AddRecipient(Recipients recipient)
        {
            Recipient = recipient;
            return this;
        }
    }
}
