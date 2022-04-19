using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalc.Models;

namespace TaxCalc.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
           

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return null;
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}