using Summer_Games_WebApi_Client.Models;
using Summer_Games_WebApi_Client.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;

namespace Summer_Games_WebApi_Client.Data
{
    public class AthleteRepository : IAthleteRepository
    {
        private readonly HttpClient client = new HttpClient();
        public AthleteRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Athlete>> GetAthletes()
        {
            HttpResponseMessage response = await client.GetAsync("api/athlete");
            if (response.IsSuccessStatusCode)
            {
                List<Athlete> athletes = await response.Content.ReadAsAsync<List<Athlete>>();
                return athletes;
            }
            else
            {
                throw new Exception("Could not access the list of Athletes.");
            }
        }
        public async Task<Athlete> GetAthlete(int AthleteID)
        {
            HttpResponseMessage response = await client.GetAsync($"api/athlete/{AthleteID}");
            if (response.IsSuccessStatusCode)
            {
                Athlete athlete = await response.Content.ReadAsAsync<Athlete>();
                return athlete;
            }
            else
            {
                throw new Exception("Could not access that Athlete.");
            }
        }
        public async Task<List<Athlete>> GetAthletesBySport(int SportID)
        {
            HttpResponseMessage response = await client.GetAsync($"api/athlete/bySport/{SportID}");
            if (response.IsSuccessStatusCode)
            {
                List<Athlete> athletes = await response.Content.ReadAsAsync<List<Athlete>>();
                return athletes;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("Cannot find any Athletes for that Sport.");
                }
                else
                {
                    throw new Exception("Could not access the list of Athletes by Sport.");
                }
            }
        }
        public async Task<List<Athlete>> GetAthletesByContingent(int ContingentID)
        {
            HttpResponseMessage response = await client.GetAsync($"api/athlete/byContingent/{ContingentID}");
            if (response.IsSuccessStatusCode)
            {
                List<Athlete> athletes = await response.Content.ReadAsAsync<List<Athlete>>();
                return athletes;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("Cannot find any Athletes for that Contingent.");
                }
                else
                {
                    throw new Exception("Could not access the list of Athletes by Contingent.");
                }
            }
        }
        public async Task AddAthlete(Athlete athleteToAdd)
        {
            var response = await client.PostAsJsonAsync("/api/athlete", athleteToAdd);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task UpdateAthlete(Athlete athleteToUpdate)
        {
            var response = await client.PutAsJsonAsync($"/api/athlete/{athleteToUpdate.ID}", athleteToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task DeleteAthlete(Athlete athleteToDelete)
        {
            var response = await client.DeleteAsync($"/api/athlete/{athleteToDelete.ID}");
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }
    }
    
}
