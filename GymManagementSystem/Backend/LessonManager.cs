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
		protected List<Lesson> _lessons = new List<Lesson>();
		public IList<Lesson> Lessons { get {  return _lessons; } }

		const string LESSONS_FILE = "BackEnd/Data/lesson.csv";

		public LessonManager()
		{
			LoadLessons();
		}
		public void LoadLessons()
		{
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LESSONS_FILE);
			string[] lines = File.ReadAllLines(filePath);
			foreach (string line in lines)
			{
				string[] column = line.Split(',');
				int code = int.Parse(column[0]);
				string name = column[1];
				string description = column[2];
				Lesson lesson = new Lesson(code, name, description);
				_lessons.Add(lesson);
			}
		}
	}
}
