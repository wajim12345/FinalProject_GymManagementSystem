//using AndroidX.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Backend
{
	internal class Member
	{
		public int Id {  get; set; }
		public string FirstName { get; set; }

		public string LastName { get; set; }
		public string Phone {  get; set; }

		public string Gender { get; set; }
		public string EmailAddress { get; set; }
		public string Address { get; set; }

		public string Dob { get; set; }

		public Member()
		{

		}

		public Member(int id, string firstname, string lastname, string phone, string gender, string email, string address, string dob)
		{
			Id = id;
			FirstName = firstname;
			LastName = lastname;
			Phone = phone;
			Gender = gender;
			EmailAddress = email;
			Address = address;
			Dob = dob;
		}
	}
}
