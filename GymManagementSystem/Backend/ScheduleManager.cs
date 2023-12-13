using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GymManagementSystem.Backend.Entities;
using MySqlConnector;

namespace GymManagementSystem.Backend
{
	internal class ScheduleManager
	{
		protected List<Schedule> _schedules = new List<Schedule>();
		protected List<Entities.Location> _locations = new List<Entities.Location>();
		public List<Schedule> Schedules { get {  return _schedules; } }
		public List<Entities.Location> Locations { get {  return _locations; } }
		const string SCHEDULE_FILE = "BackEnd/Data/schedule.csv";

		public ScheduleManager()
		{
			LoadFromDatabase();
			LoadLocationFromDatabase();
			//LoadSchedule();
		}

		/*public void LoadSchedule()
		{
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SCHEDULE_FILE);
			string[] lines = File.ReadAllLines(filePath);
			foreach (string line in lines)
			{
				string[] column = line.Split(',');
				int code = int.Parse(column[0]);
				string time = column[1];
				string location = column[2];
				int duration = int.Parse(column[3]);
				int capacity = int.Parse(column[4]);
				Schedule schedule = new Schedule(code, time, location, duration, capacity);
				_schedules.Add(schedule);
			}
		}*/

		public void LoadFromDatabase()
		{
			MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
			{
				Server = "localhost",
				UserID = "root",
				Password = "password",
				Database = "GymData",
			};
			MySqlConnection connection = new MySqlConnection(builder.ConnectionString);
			connection.Open();
			MySqlCommand command = new MySqlCommand("Select * from schedules", connection);
			MySqlDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				Schedule schedule = new Schedule
				{
					Id = Convert.ToInt32(reader["ScheduleID"]),
					Code = Convert.ToInt32(reader["code"]),
					Time = Convert.ToString(reader["Time"]),
					LocationID = Convert.ToInt32(reader["LocationID"]),
					Duration = Convert.ToInt32(reader["Duration"]),
					Capacity = Convert.ToInt32(reader["Capacity"]),
				};
				_schedules.Add(schedule);
			}

			connection.Close();
		}

		public void LoadLocationFromDatabase()
		{
			MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
			{
				Server = "localhost",
				UserID = "root",
				Password = "password",
				Database = "GymData",
			};
			MySqlConnection connection = new MySqlConnection(builder.ConnectionString);
			connection.Open();
			MySqlCommand command = new MySqlCommand("Select * from gymlocations", connection);
			MySqlDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				Entities.Location location = new Entities.Location
				{
					LocationID = Convert.ToInt32(reader["locationID"]),
					StreetAddress = Convert.ToString(reader["streetAddress"]),
					City = Convert.ToString(reader["city"]),
					Province = Convert.ToString(reader["Province"]),
					Country = Convert.ToString(reader["country"]),
					ZipCode = Convert.ToString(reader["zipCode"]),
				};
				_locations.Add(location);
			}

			connection.Close();
		}

		public string GetLocationInfo(Schedule schedule)
		{
			string locationInfo = null;
			foreach (Entities.Location location in Locations)
			{
				if (location.LocationID == schedule.LocationID)
				{
					locationInfo = $"{location.City}, {location.Province}";
				}
			}
			return locationInfo;
		}
	}
}
