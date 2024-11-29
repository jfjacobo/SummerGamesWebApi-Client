using Summer_Games_WebApi_Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summer_Games_WebApi_Client.Data
{
    public interface ISportRepository
    {
        Task<List<Sport>> GetSports();
        Task<Sport> GetSport(int ID);
    }
}
