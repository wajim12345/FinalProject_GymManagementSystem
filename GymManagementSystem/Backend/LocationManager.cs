using GymManagementSystem.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace GymManagementSystem.Backend
{
	/// <summary>
	/// Manager class for Location
	/// </summary>
	/// <remarks> Author: Qianjun Liang</remarks>
	/// <remarks>Date: Dec 12, 2023</remarks>
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
		public string SearchLocations(string search_province)
		{
			foreach (Entities.Location location in _locations)
			{
				if (location.Province == search_province)
				{
					return $"{location.LocationID}, {location.StreetAddress}, {location.City}, {location.Province}, {location.Country}, {location.ZipCode}";
				}
			}
			return "Location not found";
		}
	}
}
