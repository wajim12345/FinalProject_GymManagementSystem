﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Backend.Entities
{
	internal class Booking
	{
		public int ScheduleID { get; set; }
		public string MemberID { get; set; }

		public Booking()
		{

		}
		public Booking(int id, string memberID)
		{
			ScheduleID = id;
			MemberID = memberID;
		}
	}
}
