using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Backend.Exceptions
{
	internal class InvalidBookingException : Exception
	{
		public InvalidBookingException() 
		{
		
		}

		public InvalidBookingException(string message) : base(message) 
		{
		
		}
	}
}
