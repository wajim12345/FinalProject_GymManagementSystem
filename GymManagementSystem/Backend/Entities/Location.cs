using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GymManagementSystem.Backend.Entities
{
    public class Location
    {
        internal object input_province;

        public int LocationID { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        public Location() { }
        public Location(int location_id, string street_address, string city, string province, string country, string zip_code)
        {
            LocationID = location_id;
            StreetAddress = street_address;
            City = city;
            Province = province;
            Country = country;
            ZipCode = zip_code;
        }
    }
}
