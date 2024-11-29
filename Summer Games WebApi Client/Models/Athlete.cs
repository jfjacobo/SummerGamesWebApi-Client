using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summer_Games_WebApi_Client.Models
{
    public class Athlete
    {
        public int ID { get; set; }
        public string Summary
        {
            get
            {
                return FormalName + " - " + ACode;
            }
        }
        public string FullName
        {
            get
            {
                return FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? " " :
                        (" " + (char?)MiddleName[0] + ". ").ToUpper())
                    + LastName;
            }
        }
        public string FormalName
        {
            get
            {
                return LastName + ", " + FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? "" :
                        (" " + (char?)MiddleName[0] + ".").ToUpper());
            }
        }
        public string ACode
        {
            get
            {
                return "A:" + AthleteCode.ToString().PadLeft(7, '0');
            }
        }
        public string Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int a = today.Year - DOB.Year
                    - ((today.Month < DOB.Month || (today.Month == DOB.Month && today.Day < DOB.Day) ? 1 : 0));
                return a.ToString(); /*Note: You could add .PadLeft(3) but spaces disappear in a web page. */
            }
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AthleteCode { get; set; }
        public DateTime DOB { get; set; }
        public int Height { get; set; }
        public double Weight { get; set; }
        public string Affiliation { get; set; }
        public string MediaInfo { get; set; }
        public string Gender { get; set; }

        //Foreign keys
        public int ContingentID { get; set; }
        public Contingent Contingent { get; set; }
        public int SportID { get; set; }
        public Sport Sport { get; set; }
    }

}
