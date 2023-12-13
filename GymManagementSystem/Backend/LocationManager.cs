using GymManagementSystem.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace GymManagementSystem.Backend
{
    public class LocationManager
    {
        List<Entities.Location> _locations = new List<Entities.Location>();

        public string location_file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\gymlocation.csv");
        public string location_json = "locations.json";


        Entities.Location newLocation = new Entities.Location();

        public LocationManager()
        {
            LoadLocationsFromDB();
        }

        public int SaveLocationsToDB(Entities.Location newLocation)
        {
            int addResult = 0;
            try
            {
                MySqlConnection con = getSqlConnect();
                con.Open();
                string sql = "insert into gymlocations  (streetAddress,city,province,country,zipCode) values ('" + newLocation.StreetAddress + "','" + newLocation.City + "','" + newLocation.Province + "','" + newLocation.Country + "','" + newLocation.ZipCode + "')";
                MySqlCommand command = new MySqlCommand(sql, con);
                addResult = command.ExecuteNonQuery();
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return addResult;
        }


        private MySqlConnection getSqlConnect()
        {
            string connectionString = "server=localhost;user=root;database=gymdata;password=password;port=3306;";
            return new MySqlConnection(connectionString);
        }


        public List<Entities.Location> LoadLocationsFromDB(string province)
        {
            try
            {
                _locations = new List<Entities.Location>();
                MySqlConnection con = getSqlConnect();
                con.Open();
                string sql = "SELECT * FROM gymlocations";
                if (!string.IsNullOrEmpty(province))
                {
                    sql += " where province ='" + province + "'";
                }
                MySqlCommand command = new MySqlCommand(sql, con);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Entities.Location location = new Entities.Location
                    {
                        LocationID = reader.GetInt32("locationID"),
                        StreetAddress = reader.GetString("streetAddress"),
                        City = reader.GetString("city"),
                        Province = reader.GetString("province"),
                        Country = reader.GetString("country"),
                        ZipCode = reader.GetString("zipCode"),
                    };
                    if (!_locations.Contains(location))
                    {
                        _locations.Add(location);
                    }
                    Console.WriteLine($"LocationID: {reader["locationID"]}");
                }
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return _locations;
        }

        public void LoadLocations()
        {
            string[] lines = File.ReadAllLines(location_file);
            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                int location_id = int.Parse(columns[0]);
                string street_address = columns[1];
                string city = columns[2];
                string province = columns[3];
                string country = columns[4];
                string zip_code = columns[5];

                Entities.Location location = new(location_id, street_address, city, province, country, zip_code);
                _locations.Add(location);
            }
        }

        public void LoadLocationsFromDB()
        {
            try
            {

                string connectionString = "server=localhost;user=root;database=locations;password=0516;";
                MySqlConnection con = new MySqlConnection(connectionString);
                con.Open();
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM gymlocations", connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Entities.Location location = new Entities.Location
                            {
                                LocationID = reader.GetInt32("locationID"),
                                StreetAddress = reader.GetString("streetAddress"),
                                City = reader.GetString("city"),
                                Province = reader.GetString("province"),
                                Country = reader.GetString("country"),
                                ZipCode = reader.GetString("zipCode"),
                            };
                            _locations.Add(location);

                            // Print the loaded location for debugging
                            Console.WriteLine($"LocationID: {reader["locationID"]}");
                        }
                    }
                }
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"InvalidCastException: {ex.Message}");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Exception: {ex.Message}");
            }
        }

        public int GetLocationID()
        {
            int maxLocationId = 0;
            foreach (Entities.Location location in _locations)
            {
                if (location.LocationID > maxLocationId)
                {
                    maxLocationId = location.LocationID;
                }
            }

            return maxLocationId;
        }

        public Entities.Location AddLocation(int location_id, string street_address, string city, string province, string country, string zip_code)
        {
            Entities.Location newLocation = new Entities.Location(location_id, street_address, city, province, country, zip_code);
            _locations.Add(newLocation);
            return newLocation;
        }



        // todo
        public string SearchLocations(string searchProvince)
        {
            if (string.IsNullOrWhiteSpace(searchProvince))
            {
                return "Invalid search input";
            }

            foreach (Entities.Location location in _locations)
            {
                Console.WriteLine($"Checking location: {location.Province}");
                if (IsValidProvince(location.Province, searchProvince))
                {
                    return $"{location.LocationID}, {location.StreetAddress}, {location.City}, {location.Province}, {location.Country}, {location.ZipCode}";
                }
            }

            Console.WriteLine($"Location not found for search province: {searchProvince}");
            return "Location not found";
        }

        private bool IsValidProvince(string actualProvince, string searchProvince)
        {
            // Convert both province names to uppercase
            string actualProvinceUpper = actualProvince.ToUpper();
            string searchProvinceUpper = searchProvince.ToUpper();

            Console.WriteLine($"Checking province: {actualProvinceUpper} against {searchProvinceUpper}");

            // Check if the actualProvince matches either the full name or abbreviation
            return actualProvinceUpper.Equals(searchProvinceUpper) ||
                   actualProvinceUpper.Equals(GetAbbreviation(searchProvinceUpper));
        }

        private string GetAbbreviation(string province)
        {
            // Define a dictionary to map province names to abbreviations
            Dictionary<string, string> provinceAbbreviations = new Dictionary<string, string>
    {
        { "ALBERTA", "AB" },
        { "BRITISH COLUMBIA", "BC" },
        { "MANITOBA", "MB" },
        { "NEW BRUNSWICK", "NB" },
        { "NEWFOUNDLAND AND LABRADOR", "NL" },
        { "NOVA SCOTIA", "NS" },
        { "ONTARIO", "ON" },
        { "PRINCE EDWARD ISLAND", "PE" },
        { "QUEBEC", "QC" },
        { "SASKATCHEWAN", "SK" }
    };

            string provinceUpper = province.ToUpper();

            if (provinceAbbreviations.TryGetValue(provinceUpper, out string abbreviation))
            {
                return abbreviation.ToUpper();
            }

            Console.WriteLine($"Abbreviation not found for {provinceUpper}");

            return provinceUpper;
        }
    }
}
