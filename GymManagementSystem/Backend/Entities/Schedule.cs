using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Backend.Entities
{
	internal class Schedule
	{
		public int Id { get; set; }
		public int Code { get; set; }
		public string Time {  get; set; }
		public int LocationID { get; set; }
		public int Duration { get; set; }
		public int Capacity { get; set; }

		public Schedule()
		{

		}

		public Schedule(int code, string time, int locationID, int duration, int capacity)
		{
			Code = code;
			Time = time;
			LocationID = locationID;
			Duration = duration;
			Capacity = capacity;
		}

		public string EndTime()
		{
			DateTime time = DateTime.Parse(Time);
			DateTime newTime = time .AddMinutes(Duration);
			return newTime.ToString("HH:mm");
		}
		
	}
}
