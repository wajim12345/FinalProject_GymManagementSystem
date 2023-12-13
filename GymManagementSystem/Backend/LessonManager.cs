using GymManagementSystem.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace GymManagementSystem.Backend
{
	internal class LessonManager
	{
		protected List<Lesson> _lessons = new List<Lesson>();
		public IList<Lesson> Lessons { get {  return _lessons; } }
		public LessonManager()
		{
			LoadFromDatabase();
			//LoadLessons();
		}

		public void LoadFromDatabase()
		{
			MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
			{
				Server = "localhost",
				UserID = "root",
				Password = "password",
				Database = "GymData",
			};
			MySqlConnection connection = new MySqlConnection(builder.ConnectionString);
			connection.Open();
			MySqlCommand command = new MySqlCommand("Select * from lessons", connection);
			MySqlDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				Lesson lesson = new Lesson
				{
					Code = Convert.ToInt32(reader["code"]),
					Name = Convert.ToString(reader["name"]),
					Description = Convert.ToString(reader["description"]),
				};
				_lessons.Add(lesson);
			}

			connection.Close();
		}
		/*public void LoadLessons()
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
		}*/
	}
}
