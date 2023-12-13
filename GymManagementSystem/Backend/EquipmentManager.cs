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
	/// <summary>
	/// Manager class for Equipment
	/// </summary>
	/// <remarks> Author: Zac Ruff</remarks>
	/// <remarks>Date: Dec 12, 2023</remarks>
	internal class EquipmentManager
    {
        List<Equipment> _equipmentList = new List<Equipment>();
        const string EQUIPMENT_FILE = "Data/equipment.csv";
        const string EQUIPMENT_JSON = "equipment.json";

        int maxId = 0;
        Equipment newEquipment = new Equipment();
        public List<Equipment> EquipmentList { get { return _equipmentList; } }

        public EquipmentManager()
        {
            LoadEquipmentFromDB();
        }

        public void LoadEquipment()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, EQUIPMENT_FILE);
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] column = line.Split(',');

                int id = int.Parse(column[0]);
                string name = column[1];
                string type = column[2];
                string weight = column[3];

                Equipment equipment = new(id, name, type, weight);
            }
        }

        public void LoadEquipmentFromDB()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                UserID = "root",
                Password = "password",
                Database = "gymdata",
            };
            MySqlConnection connection = new(builder.ConnectionString);
            connection.Open();
            MySqlCommand command = new("Select * from equipment", connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Equipment equipment = new Equipment
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Type = reader.GetString("type"),
                    Weight = reader.GetString("weight"),
                };
                _equipmentList.Add(equipment);
            }
            connection.Close();
        }

        public Equipment AddEquipment(int id, string name, string type, string weight)
        {

            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidAddingException("Name cannot be empty.");
            else if (string.IsNullOrWhiteSpace(type))
                throw new InvalidAddingException("Type cannot be empty.");
            else if (string.IsNullOrWhiteSpace(weight))
                throw new InvalidAddingException("Weight cannot be empty. Enter 'N/A' if the type is machine.");
            else
            {
                newEquipment = new(id, name, type, weight);
                _equipmentList.Add(newEquipment);
                return newEquipment;
            }
        }

        public Equipment SearchEquipment(string inputID, string name)
        {
            foreach (Equipment equipment in _equipmentList)
            {
                if (string.IsNullOrEmpty(inputID) && string.IsNullOrEmpty(name))
                {
                    throw new InvalidSearchingExecption("Please enter member ID or name.");
                }
                // if user enter both id and name
                else if (!string.IsNullOrEmpty(inputID) && !string.IsNullOrEmpty(name))
                {
                    int id = int.Parse(inputID);
                    if (equipment.Id == id && (equipment.Name == name))
                    {
                        return equipment;
                    }
                }
                // if user only enter id
                else if (string.IsNullOrEmpty(name))
                {
                    int id = int.Parse(inputID);
                    if (equipment.Id == id)
                    {
                        return equipment;
                    }
                }
                // if user only enter name
                else if (string.IsNullOrEmpty(inputID))
                {
                    if ((equipment.Name == name))
                    {
                        return equipment;
                    }
                }
            }
            return null;
        }
        public int GetMaxId()
        {
            foreach (Equipment equipment in _equipmentList)
            {
                if (equipment.Id > maxId)
                {
                    maxId = equipment.Id;
                }
            }
            return maxId;
        }

        public void SaveEquipment()
        {
            string equipment = JsonSerializer.Serialize(_equipmentList);
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, EQUIPMENT_JSON);
            File.WriteAllText(path, equipment);
        }

        public void SaveToDB()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                UserID = "root",
                Password = "password",
                Database = "gymdata",
            };
            MySqlConnection connection = new(builder.ConnectionString);
            connection.Open();

            int id = newEquipment.Id;
            string name = newEquipment.Name;
            string type = newEquipment.Type;
            string weight = newEquipment.Weight;

            string sql = $"INSERT INTO equipment VALUES ({id}, '{name}', '{type}', '{weight}')";
            MySqlCommand command = new(sql, connection);
            command.ExecuteNonQuery();

            connection.Close();


        }
    }
}