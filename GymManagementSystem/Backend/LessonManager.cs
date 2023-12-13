using GymManagementSystem.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace GymManagementSystem.Backend
{
	/// <summary>
	/// Manager class for Lesson
	/// </summary>
	/// <remarks> Author: Jim Wang</remarks>
	/// <remarks>Date: Dec 12, 2023</remarks>
	internal class LessonManager
	{
		protected List<Lesson> _lessons = new List<Lesson>();
		public IList<Lesson> Lessons { get {  return _lessons; } }
		public LessonManager()
		{
			LoadFromDatabase();
			//LoadLessons();
		}

		/// <summary>
		/// Connects to Database and read the data convert into a list of lesson objects
		/// </summary>
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
		
	}
}
