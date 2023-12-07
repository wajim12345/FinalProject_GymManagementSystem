using GymManagementSystem.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Backend
{
	internal class LessonManager
	{
		protected List<Lesson> _lessons;
		public IList<Lesson> Lessons { get {  return _lessons; } }

		const string LESSONS_FILE = "lesson.csv";

		public LessonManager()
		{
			LoadLessons();
		}
		public void LoadLessons()
		{
			
		}
	}
}
