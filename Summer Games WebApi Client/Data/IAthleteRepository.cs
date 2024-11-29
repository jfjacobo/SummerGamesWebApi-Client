using Summer_Games_WebApi_Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Summer_Games_WebApi_Client.Data
{
    public interface IAthleteRepository
    {
        Task<List<Athlete>> GetAthletes();
        Task<Athlete> GetAthlete(int ID);
        Task<List<Athlete>> GetAthletesBySport(int SportID);
        Task<List<Athlete>> GetAthletesByContingent(int ContingentID);
        Task AddAthlete(Athlete athleteToAdd);
        Task UpdateAthlete(Athlete athleteToUpdate);
        Task DeleteAthlete(Athlete athleteToDelete);
    }
}
