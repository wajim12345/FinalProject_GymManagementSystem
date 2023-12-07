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
		public string MemberName { get; set; }

		public Booking()
		{

		}
		public Booking(Schedule schedule,string memberName, string memberID)
		{
			Schedule = schedule;
			MemberName = memberName;
			MemberID = memberID;
		}
	}
}
