using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GymManagementSystem.Backend.Entities;
using GymManagementSystem.Backend.Exceptions;
using MySqlConnector;

namespace GymManagementSystem.Backend
{
    internal class MemberManager
    {
        public List<Member> Members { get { return _members; } }

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


        /// <summary>
        /// Load members' information from csv file.
        /// </summary>
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


        /// <summary>
        /// Load the members' information from database
        /// </summary>
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


        /// <summary>
        /// Get the max ID from current members.
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// Add a new member based on user input information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="phone"></param>
        /// <param name="gender"></param>
        /// <param name="email"></param>
        /// <param name="address"></param>
        /// <param name="dob"></param>
        /// <returns></returns>
        /// <exception cref="InvalidAddingException"></exception>
        public Member AddMember(int id, string firstname, string lastname, string phone, string gender, string email, string address, string dob)
        {
            foreach (Member member in _members)
            {
                if (member.EmailAddress == email)
                {
                    throw new InvalidAddingException("Email address already exists.");
                }
            }

            if (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
            {
                throw new InvalidAddingException("Name cannot be empty.");
            }
            else if (string.IsNullOrWhiteSpace(phone))
            {
                throw new InvalidAddingException("Phone cannot be empty.");
            }
            else if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidAddingException("Email cannot be empty.");
            }
            else
            {
                newMember = new Member(id, firstname, lastname, phone, gender, email, address, dob);
                _members.Add(newMember);
                return newMember;
            }

        }


        /// <summary>
        /// Search member information based on user input.
        /// </summary>
        /// <param name="inputID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="InvalidSearchingExecption"></exception>
        public Member SearchMember(string inputID, string name)
        {
            foreach (Member member in _members)
            {
                if (string.IsNullOrEmpty(inputID) && string.IsNullOrEmpty(name))
                {
                    throw new InvalidSearchingExecption("Please enter member ID or name.");
                }
                // if user enter both id and name
                else if (!string.IsNullOrEmpty(inputID) && !string.IsNullOrEmpty(name))
                {
                    int id = int.Parse(inputID);
                    if (member.Id == id && (member.FirstName + ' ' + member.LastName == name))
                    {
                        return member;
                    }
                }
                // if user only enter id
                else if (string.IsNullOrEmpty(name))
                {
                    int id = int.Parse(inputID);
                    if (member.Id == id)
                    {
                        return member;
                    }
                }
                // if user only enter name
                else if (string.IsNullOrEmpty(inputID))
                {
                    if ((member.FirstName + ' ' + member.LastName == name))
                    {
                        return member;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Save the new member information to Json file.
        /// </summary>
        public void SaveMember()
        {
            string members = JsonSerializer.Serialize(_members);
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MEMBER_JSON_FILE);
            File.WriteAllText(path, members);
        }


        /// <summary>
        /// Save the new member information to the database.
        /// </summary>
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
