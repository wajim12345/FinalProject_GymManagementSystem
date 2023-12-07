using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GymManagementSystem.Backend.Entities;

namespace GymManagementSystem.Backend
{
	internal class BookingManager
	{
		protected List<Booking> _bookings = new List<Booking>();
		const string BOOKING_JSON_FILE = "booing.json";
		public List<Booking> Bookings { get {  return _bookings; } }

		public void SaveBooking()
		{
			string booking = JsonSerializer.Serialize(_bookings);
			string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BOOKING_JSON_FILE);
			File.WriteAllText(path, booking);
		}

		public void LoadBooking()
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
		}

		public int AvailableCapacity(Schedule schedule)
		{
			int capBooked = 0;
			if (Bookings.Count >0)
			{
				foreach (Booking booking in Bookings)
				{
					if (booking.Schedule.Code == schedule.Code)
					{
						capBooked++;
					}
				}
			}
			int remainingCap = schedule.Capacity - capBooked;
			return remainingCap;
		}

		public Booking BookLesson(Schedule schedule, string name, string id)
		{
			Booking booking = new Booking(schedule, name, id);
			_bookings.Add(booking);
			SaveBooking();
			return booking;
		}
	}
}
