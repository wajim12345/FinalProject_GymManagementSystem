using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GymManagementSystem.Backend.Entities;
using GymManagementSystem.Backend.Exceptions;
using MySqlConnector;

namespace GymManagementSystem.Backend
{
	internal class BookingManager
	{
		protected List<Booking> _bookings = new List<Booking>();
		public List<Booking> Bookings { get {  return _bookings; } }
		public BookingManager() 
		{
			LoadFromDatabase();
		}


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
			MySqlCommand command = new MySqlCommand("Select * from bookings", connection);
			MySqlDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				Booking booking = new Booking
				{
					ScheduleID = Convert.ToInt32(reader["ScheduleID"]),
					MemberID = Convert.ToString(reader["memberID"]),
					
				};
				_bookings.Add(booking);
			}

			connection.Close();
		}

		public void SaveToDatabase()
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
			MySqlTransaction transaction = connection.BeginTransaction();
			transaction.Commit();
			connection.Close();
			
		}
		/*public void SaveBooking()
		{
			string booking = JsonSerializer.Serialize(_bookings);
			string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BOOKING_JSON_FILE);
			File.WriteAllText(path, booking);
		}*/

		/*public void LoadBooking()
		{
			string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BOOKING_JSON_FILE);
			if(!File.Exists(path))
			{
				return;
			}
			else
			{
				string content = File.ReadAllText(path);
				_bookings = JsonSerializer.Deserialize<List<Booking>>(content); 
			}
		}*/

		public int AvailableCapacity(Schedule schedule)
		{
			int capBooked = 0;
			if (Bookings.Count >0)
			{
				foreach (Booking booking in Bookings)
				{
					if (booking.ScheduleID == schedule.Id)
					{
						capBooked++;
					}
				}
			}
			int remainingCap = schedule.Capacity - capBooked;
			return remainingCap;
		}

		public void BookLesson(int scheduleID, string memberID)
		{
			Booking booking = new Booking(scheduleID, memberID);
			_bookings.Add(booking);

			MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
			{
				Server = "localhost",
				UserID = "root",
				Password = "password",
				Database = "GymData",
			};

			using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
			{

				try
				{
					connection.Open();
					MySqlCommand command = new MySqlCommand("INSERT INTO bookings (scheduleID, memberID) VALUES (@ScheduleID, @MemberID)", connection);
					command.Parameters.AddWithValue("@ScheduleID", scheduleID);
					command.Parameters.AddWithValue("@MemberID", memberID);
					command.ExecuteNonQuery();
				}
				catch
				{
					throw new InvalidBookingException("Booking Failed");
				}
				finally
				{
					connection.Close();
				}
			}
		}

	}
}
