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
    public class ContingentRepository : IContingentRepository 
    {
        private readonly HttpClient client = new HttpClient();
        public ContingentRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Contingent>> GetContingents()
        {
            HttpResponseMessage response = await client.GetAsync("api/contingent");
            if (response.IsSuccessStatusCode)
            {
                List<Contingent> contingents = await response.Content.ReadAsAsync<List<Contingent>>();
                return contingents;
            }
            else
            {
                throw new Exception("Could not access the list of Contingents.");
            }
        }
        public async Task<Contingent> GetContingent(int ContingentID)
        {
            HttpResponseMessage response = await client.GetAsync($"api/contingent/{ContingentID}");
            if (response.IsSuccessStatusCode)
            {
                Contingent contingent = await response.Content.ReadAsAsync<Contingent>();
                return contingent;
            }
            else
            {
                throw new Exception("Could not access that Contingent.");
            }
        }
    }
}
