using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TaxCalc.Models;

namespace TaxCalc.Services
{
    public class FirebaseDBService
    {
        private HttpClient Http { get; set; }
       // private FirebaseClient Client { get; set; }

        private static string CompanyAddressesTable = "CompanyAddresses.json";
        private static string InventoryItemsTable = "InventoryItems.json";

        public FirebaseDBService(string url)
        {
            Http = new HttpClient();
            Http.BaseAddress = new Uri(url);
            //Client = new FirebaseClient(url);
        }

        public async Task<bool> UpdateCompanyAddresses(List<AddressRef> addr)
        {
            //await Client.Child(CompanyAddressesTable).PostAsync(addr);
            //return true;
            var response = await Http.PutAsJsonAsync(CompanyAddressesTable, addr);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<AddressRef>> GetCompanyAddresses()
        {
            //var results = Client.Child(CompanyAddressesTable).AsObservable<AddressRef>().AsObservableCollection();
            //return results;
            var response = await Http.GetAsync(CompanyAddressesTable);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<AddressRef>>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return new List<AddressRef>();
            }
        }

        public async Task<bool> UpdateInventoryItems(List<Item> item)
        {
            //await Client.Child(InventoryItemsTable).PostAsync(item);
            //return true;
            var response = await Http.PutAsJsonAsync(InventoryItemsTable, JsonConvert.SerializeObject(item));
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Item>> GetInventoryItems()
        {
            //var items = Client.Child(InventoryItemsTable).AsObservable<Item>().AsObservableCollection();
            //return items;
            var response = await Http.GetAsync(InventoryItemsTable);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Item>>(responseString);
            }
            else
            {
                return new List<Item>();
            }
        }
    }
}
