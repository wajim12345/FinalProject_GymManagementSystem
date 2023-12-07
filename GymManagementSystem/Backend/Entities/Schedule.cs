using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Backend.Entities
{
	internal class Schedule
	{
		public int Code { get; set; }
		public string Time {  get; set; }
		public string Location { get; set; }
		public int Duration { get; set; }
		public int Capacity { get; set; }

		public Schedule()
		{

		}

		public Schedule(int code, string time, string location, int duration, int capacity)
		{
			Code = code;
			Time = time;
			Location = location;
			Duration = duration;
			Capacity = capacity;
		}
	}
}
