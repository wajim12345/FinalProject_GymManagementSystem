using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MySqlConnector;

namespace GymManagementSystem.Backend
{
	internal class MemberManager
	{
		List<Member> _members = new List<Member>();

		const string MEMBER_FILE = "Data/members.csv";
		const string MEMBER_JSON_FILE = "members.json";

		int maxId = 0;
		Member newMember = new Member();

		public MemberManager()
		{
			//LoadMembers();
			LoadMemberFromDatabase();

        }

		public void LoadMembers()
		{
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MEMBER_FILE);
			string[] lines = File.ReadAllLines(filePath);

			lines = lines.Skip(0).ToArray();

			foreach (string line in lines)
			{
				string[] column = line.Split(',');

				int id = int.Parse(column[0]);
				string firstName = column[1];
				string lastName = column[2];
                string phone = column[3];
                string gender = column[4];
				string emailAddress = column[5];
				string address = column[6];
				string dob = column[7];
				Member member = new Member(id, firstName, lastName, phone, gender, emailAddress, address, dob);
				_members.Add(member);
			}
		}


		public void LoadMemberFromDatabase()
		{
			MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
				Server = "localhost",
				UserID = "root",
				Password = "password",
				Database = "gymdata",
			};
			MySqlConnection connection = new MySqlConnection(builder.ConnectionString);
			connection.Open();
			MySqlCommand command = new MySqlCommand("Select * from members", connection);
			MySqlDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				Member member = new Member
				{
					Id = reader.GetInt32("id"),
					FirstName = reader.GetString("firstName"),
					LastName = reader.GetString("lastName"),
					Phone = reader.GetString("phone"),
					Gender = reader.GetString("gender"),
					EmailAddress = reader.GetString("emailAddress"),
					Address = reader.GetString("address"),
					Dob = reader.GetString("dateOfBirth"),
				};
				_members.Add(member);
			}

			connection.Close();
		}

		public int GetMaxId()
		{
			foreach (Member member in _members)
			{
				if (member.Id > maxId)
				{
					maxId = member.Id;
				}
			}
			return maxId;
		}


		public Member AddMember(int id, string firstname, string lastname, string phone, string gender, string email, string address, string dob)
		{
			foreach(Member member in _members)
			{
				if (member.EmailAddress == email)
				{
                    throw new ApplicationException("Email address already exists.");
                }
			}

			if (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
			{
				throw new Exception("Name cannot be empty.");
			}
			else if (string.IsNullOrWhiteSpace(phone))
			{
				throw new Exception("Phone cannot be empty.");
			}
			else if (!string.IsNullOrWhiteSpace(email))
			{
				throw new Exception("Email cannot be empty.");
			}
			else
			{
                newMember = new Member(id, firstname, lastname, phone, gender, email, address, dob);
                _members.Add(newMember);
                return newMember;
            }

		}


		public Member SearchMemberById(int id)
		{
			foreach (Member member in _members)
			{
				if (member.Id == id)
				{
					return member;
				}
			}
			return null;
		}


		public void SaveMember()
		{
			string members = JsonSerializer.Serialize(_members);
			string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MEMBER_JSON_FILE);
			File.WriteAllText(path, members);
		}

		public void SaveToDatabase()
		{
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                UserID = "root",
                Password = "password",
                Database = "gymdata",
            };
            MySqlConnection connection = new MySqlConnection(builder.ConnectionString);
            connection.Open();

			int id = newMember.Id;
			string firstName = newMember.FirstName;
            string lastName = newMember.LastName;
            string phone = newMember.Phone;
            string gender = newMember.Gender;
            string emailAddress = newMember.EmailAddress;
            string address = newMember.Address;
            string dob = newMember.Dob;

			string sql = $"INSERT INTO members VALUES ({id}, '{firstName}', '{lastName}','{phone}','{gender}','{emailAddress}','{address}','{dob}')";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.ExecuteNonQuery();

            connection.Close();
        }
	}
}
