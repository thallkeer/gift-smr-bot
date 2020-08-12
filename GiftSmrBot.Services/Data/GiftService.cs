using GiftSmrBot.Core;
using GiftSmrBot.Core.DataInterfaces;
using GiftSmrBot.Core.Extensions;
using GiftSmrBot.Core.Models;
using GiftSmrBot.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiftSmrBot.Services
{
    public class GiftService : EntityService<Gift, int>, IGiftService
    {
        public GiftService(ApplicationContext context) : base(context)
        {
        }

        public void Dispose()
        {
            
        }

        public IEnumerable<Gift> GetGiftsByCriteria(GiftCriteriaBuilder giftCriteria)
        {
            var criteria = giftCriteria;
            int lowerPrice = criteria.Price.GetLower() * 100;
            int upperPrice = criteria.Price.GetUpper() * 100;
            var gifts = GetAll().Where(g =>
                               g.AgeCategory == criteria.Age
                            && g.Recipient == criteria.Recipient
                            && g.Price >= lowerPrice && g.Price <= upperPrice);
            return gifts;
        }
    }
}
