using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Summer_Games_WebApi_Client.Models
{
    public class Sport
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
     
        public ICollection<Athlete> Athletes = new HashSet<Athlete>();

    }
}
