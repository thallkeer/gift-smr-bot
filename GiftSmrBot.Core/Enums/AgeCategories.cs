using System.ComponentModel.DataAnnotations;

namespace GiftSmrBot.Core.Enums
{
    public enum AgeCategories
    {
        [Display(Name = "До 14")]
        Before_14 = 1,
        [Display(Name = "14-18")]
        _14_18 = 2,
        [Display(Name = "18-25")]
        _18_25 = 3,
        [Display(Name = "26-40")]
        _26_40 = 4,
        [Display(Name = "40-60")]
        _40_60,
        [Display(Name = "После 60")]
        Above_60
    }
}
