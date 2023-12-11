using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Backend.Entities
{
	internal class Member
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Gender { get; set; }
		public string Phone { get; set; }
		public string EmailAddress { get; set; }
		public string Address { get; set; }
		public string Dob { get; set; }

		public Member()
		{

		}

		public Member(int id, string firstName, string lastName, string gender, string phone, string email, string address, string dob)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			Gender = gender;
			Phone = phone;
			EmailAddress = email;
			Address = address;
			Dob = dob;
		}
	}
}
