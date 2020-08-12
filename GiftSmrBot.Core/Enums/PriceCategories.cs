using System;
using System.ComponentModel.DataAnnotations;

namespace GiftSmrBot.Core.Enums
{
    public class PriceCategoryValueAttribute : Attribute
    {
        public int Lower { get; set; }
        public int Upper { get; set; }
        
        public PriceCategoryValueAttribute(int lower, int upper)
        {
            Lower = lower;
            Upper = upper;
        }
    }

    public enum PriceCategories
    {
        [Display(Name = "500-1000")]
        [PriceCategoryValue(500,1000)]
        _500_1000,
        [Display(Name = "1000-2000")]
        [PriceCategoryValue(1000, 2000)]
        _1000_2000,
        [Display(Name = "2000-3000")]
        [PriceCategoryValue(2000, 3000)]
        _2000_3000,
        [Display(Name = "Дороже 3000")]
        [PriceCategoryValue(3000, int.MaxValue)]
        Above3000
    }   
}
