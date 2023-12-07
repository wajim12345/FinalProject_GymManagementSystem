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
		protected List<Schedule> schedules;
		public List<Schedule> Schedules { get {  return schedules; } }
		const string SCHEDULE_FILE = "schedule.csv";
		
		
	}
}
