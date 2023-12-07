using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Backend.Entities
{
	internal class Booking
	{
		public Schedule Schedule { get; set; }
		public string MemberID { get; set; }

		public Booking()
		{

		}
		public Booking(Schedule schedule, string memberID)
		{
			Schedule = schedule;
			MemberID = memberID;
		}
	}
}
