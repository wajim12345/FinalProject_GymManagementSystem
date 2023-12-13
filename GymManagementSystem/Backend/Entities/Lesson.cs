using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Backend.Entities
{
	internal class Lesson
	{
		public int Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public Lesson()
		{

		}

		public Lesson(int code, string name, string description)
		{
			Code = code;
			Name = name;
			Description = description;
		}
	}
}
