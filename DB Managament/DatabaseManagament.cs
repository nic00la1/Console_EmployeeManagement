using Console_EmployeeManagement.Models;
using ConsoleTables;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mysqlx.Expect.Open.Types;

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
                worker.Id = Convert.ToInt32(reader["id_worker"]);
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
                var stanowisko = worker.IdRole == 1 ? "Admin" 
                    : (worker.IdRole == 2 ? "Rekruter" 
                    : ( worker.IdRole == 3 ? "Programista" 
                    : "Dzial HR"));

                table.AddRow(worker.Id, worker.Name,
                worker.Surname, stanowisko);
            }

            table.Options.EnableCount = false; // Wylaczenie numeracji wierszy
            table.Write(); // Wyswietla tabele 
        }

        public void WyswietlPracownika(int id)
        {
            string query = $"SELECT * FROM workers WHERE id_worker = {id}";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Worker worker = new Worker();
                    worker.Id = Convert.ToInt32(reader["id_worker"]);
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
                    var stanowisko = worker.IdRole == 1 ? "Admin"
                        : (worker.IdRole == 2 ? "Rekruter"
                        : (worker.IdRole == 3 ? "Programista"
                        : "Dzial HR"));

                    table.AddRow(worker.Id, worker.Name,
                                       worker.Surname, stanowisko);
                }

                table.Options.EnableCount = false; // Wylaczenie numeracji wierszy
                table.Write(); // Wyswietla tabele 
            }
            else
            {
                Console.WriteLine("Nie ma pracownika o podanym ID.");
            }
        }

        public void WyswietlStanowiska()
        {
            // Wyswietlenie stanowisk + ile osob pracuje na danym stanowisku
            string query = "SELECT roles.role_name, COUNT(workers.id_role) as ilosc_pracownikow " +
                "FROM roles " +
                "LEFT JOIN workers ON roles.id_role = workers.id_role " +
                "GROUP BY roles.role_name";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader["role_name"] + " - " + reader["ilosc_pracownikow"]);
            }

            conn.Close();
        }

        public void DodajPracownika(Worker worker)
        {
            Console.WriteLine("Podaj imie: ");
            worker.Name = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko: ");
            worker.Surname = Console.ReadLine();
            Console.WriteLine("Podaj wiek: ");
            worker.Age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Podaj id stanowiska: ");
            worker.IdRole = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Podaj date zatrudnienia: ");
            worker.HireDate = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("Czy pracownik jest zatrudniony? (1 - tak, 0 - nie): ");
            worker.IsWorking = Convert.ToInt16(Console.ReadLine());

            string query = $"INSERT INTO workers (name, surname, age, id_role, hire_date, is_working) " +
                $"VALUES ('{worker.Name}', '{worker.Surname}', {worker.Age}, {worker.IdRole}, '{worker.HireDate}', {worker.IsWorking})";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            Console.WriteLine("Dodano nowego pracownika.");
        }
    }
}
