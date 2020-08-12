using System.ComponentModel.DataAnnotations;

namespace GiftSmrBot.Core.Enums
{
    public enum Recipients
    {
        [Display(Name = "Девушка")]
        Girlfriend = 1,
        [Display(Name = "Парень")]
        Boyfriend,
        [Display(Name = "Мама")]
        Mother,
        [Display(Name = "Папа")]
        Father,
        [Display(Name = "Тётя")]
        Aunt,
        [Display(Name = "Дядя")]
        Uncle       
    }
}
