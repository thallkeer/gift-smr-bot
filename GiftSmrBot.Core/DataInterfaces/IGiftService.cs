using GiftSmrBot.Core.Models;
using System;
using System.Collections.Generic;

namespace GiftSmrBot.Core.DataInterfaces
{
    public interface IGiftService : IEntityService<Gift, int>, IDisposable
    {
        IEnumerable<Gift> GetGiftsByCriteria(GiftCriteriaBuilder giftCriteria);
    }
}
