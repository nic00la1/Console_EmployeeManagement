using Console_EmployeeManagement.Models;
using ConsoleTables;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_EmployeeManagement.DB_Managament
{
    public class DatabaseManagament
    {
        public List<Worker> lista_pracownikow = new List<Worker>(); // Lista wszystkich pracownikow

        string connectionString = 
            $"server = {Properties.Resources.server};" +
           $"uid={Properties.Resources.uid};" +
            $"pwd={Properties.Resources.pwd};" +
            $"database={Properties.Resources.database}";
        MySqlConnection conn;

        // Konstruktor
        public DatabaseManagament()
        {
            conn = new MySqlConnection(connectionString);
        }

        public void WyswietlPracownikow()
        {
            string query = "SELECT * FROM workers";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                Worker worker = new Worker();
                worker.Id = Convert.ToInt32(reader["id"]);
                worker.Name = reader["name"].ToString();
                worker.Surname = reader["surname"].ToString();
                worker.Age = Convert.ToInt32(reader["age"]);
                worker.IdRole = Convert.ToInt32(reader["id_role"]);
                worker.HireDate = Convert.ToDateTime(reader["hire_date"]);
                worker.IsWorking = Convert.ToInt16(reader["is_working"]); // tiny int

                lista_pracownikow.Add(worker);
            }

            conn.Close();

            //  Uzywam paczke z NugetPackage - ,,ConsoleTables"
            var table = new ConsoleTable("Id", "Imie",
                "Nazwisko", "Stanowisko");

            foreach (Worker worker in lista_pracownikow)
            {
                table.AddRow(worker.Id, worker.Name,
                    worker.Surname, worker.IdRole);
            }

            table.Options.EnableCount = false; // Wylaczenie numeracji wierszy
            table.Write(); // Wyswietla tabele 
        }
    }
}
