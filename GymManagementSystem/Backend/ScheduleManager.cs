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
	/// <summary>
	/// Manager class for Schedule
	/// </summary>
	/// <remarks> Author: Jim Wang</remarks>
	/// <remarks>Date: Dec 12, 2023</remarks>
	internal class ScheduleManager
	{
		protected List<Schedule> _schedules = new List<Schedule>();
		protected List<Entities.Location> _locations = new List<Entities.Location>();
		public List<Schedule> Schedules { get {  return _schedules; } }
		public List<Entities.Location> Locations { get {  return _locations; } }

		public ScheduleManager()
		{
			LoadFromDatabase();
			LoadLocationFromDatabase();

		}


		/// <summary>
		/// Connects to Database and read the data convert into a list of schedule objects
		/// </summary>
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
		/// <summary>
		/// Connects to Database and read the data convert into a list of location objects
		/// </summary>
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
