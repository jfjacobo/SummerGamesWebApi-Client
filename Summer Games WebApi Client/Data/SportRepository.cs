using Summer_Games_WebApi_Client.Models;
using Summer_Games_WebApi_Client.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Summer_Games_WebApi_Client.Data
{
    public class SportRepository : ISportRepository
    {
        private readonly HttpClient client = new HttpClient();
        public SportRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Sport>> GetSports()
        {
            HttpResponseMessage response = await client.GetAsync("api/sport");
            if (response.IsSuccessStatusCode)
            {
                List<Sport> sports = await response.Content.ReadAsAsync<List<Sport>>();
                return sports;
            }
            else
            {
                throw new Exception("Could not access the list of Sports.");
            }
        }
        public async Task<Sport> GetSport(int SportID)
        {
            HttpResponseMessage response = await client.GetAsync($"api/sport/{SportID}");
            if (response.IsSuccessStatusCode)
            {
                Sport sport = await response.Content.ReadAsAsync<Sport>();
                return sport;
            }
            else
            {
                throw new Exception("Could not access that Sport.");
            }
        }
    }
}
