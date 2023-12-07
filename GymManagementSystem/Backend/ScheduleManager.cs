using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementSystem.Backend.Entities;

namespace GymManagementSystem.Backend
{
	internal class ScheduleManager
	{
		protected List<Schedule> _schedules = new List<Schedule>();
		public List<Schedule> Schedules { get {  return _schedules; } }
		const string SCHEDULE_FILE = "BackEnd/Data/schedule.csv";
		
		public ScheduleManager()
		{
			LoadSchedule();
		}

		public void LoadSchedule()
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
		}
	}
}
