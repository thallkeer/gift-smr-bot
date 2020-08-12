using GiftSmrBot.Core.Models;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GiftSmrBot.Core;
using GiftSmrBot.Core.Extensions;

namespace GiftSmrBot.Services
{
    public class GiftJsonService 
    {
        private readonly string _filePath;
        private ConcurrentBag<Gift> _data;
        public GiftJsonService(IOptions<DbSettings> dbOptions)
        {
            _filePath = dbOptions.Value.Path;
            _data = JsonConvert.DeserializeObject<ConcurrentBag<Gift>>(File.ReadAllText(_filePath));
        }

        private async Task SaveData()
        {
            string json = JsonConvert.SerializeObject(_data);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task Add(Gift gift)
        {
            if (gift.Id != 0)
                return;
            gift.Id = _data.Count == 0 ? 1 : _data.Max(g => g.Id) + 1;
            _data.Add(gift);
            await SaveData();
        }

        public async Task<bool> Delete(int id)
        {
            return await Delete(_data.FirstOrDefault(gift => gift.Id == id));
        }

        public async Task<bool> Delete(Gift gift)
        {
            if (gift == null)
                throw new NullReferenceException("No such gift to delete!");
            var listOfData = _data.ToList();
            if (listOfData.Remove(gift))
            {
                _data = new ConcurrentBag<Gift>(listOfData);
                await SaveData();
                return true;
            }
            return false;
        }

        public IQueryable<Gift> GetAll()
        {
            return _data.AsQueryable();
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
