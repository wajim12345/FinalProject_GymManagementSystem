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
	/// <summary>
	/// Manager class for Booking
	/// </summary>
	/// <remarks> Author: Jim Wang</remarks>
	/// <remarks>Date: Dec 12, 2023</remarks>
	internal class BookingManager
	{
		protected List<Booking> _bookings = new List<Booking>();
		public List<Booking> Bookings { get {  return _bookings; } }
		public BookingManager() 
		{
			LoadFromDatabase();
		}


		/// <summary>
		/// Connects to Database and read the data convert into a list of booking objects
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

		/// <summary>
		/// Connects to database and save the data
		/// </summary>
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
		
		/// <summary>
		/// Checks avalability of a schedule
		/// </summary>
		/// <param name="schedule"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Create a booking object in the list and also create a row in booking table in database
		/// </summary>
		/// <param name="scheduleID"></param>
		/// <param name="memberID"></param>
		/// <exception cref="InvalidBookingException"></exception>
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
				catch(InvalidBookingException e)
				{
					throw new InvalidBookingException("Booking Failed" + e);
				}
				finally
				{
					connection.Close();
				}
			}
		}

	}
}
