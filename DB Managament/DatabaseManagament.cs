using Console_EmployeeManagement.Models;
using ConsoleTables;
using MySql.Data.MySqlClient;

namespace Console_EmployeeManagement.DB_Managament
{
    public class DatabaseManagament
    {
        public List<Worker> ListaPracownikow = new List<Worker>(); // Lista wszystkich pracownikow

        string _connectionString = 
            $"server = {Properties.Resources.server};" +
           $"uid={Properties.Resources.uid};" +
            $"pwd={Properties.Resources.pwd};" +
            $"database={Properties.Resources.database}";
        MySqlConnection _conn;

        // Konstruktor
        public DatabaseManagament()
        {
            _conn = new MySqlConnection(_connectionString);
        }

        public void WyswietlPracownikow()
        {
            string query = "SELECT * FROM workers";
            MySqlCommand cmd = new MySqlCommand(query, _conn);

            _conn.Open();
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
                worker.Login = reader["login"].ToString();

                ListaPracownikow.Add(worker);
            }

            _conn.Close();

            // Jesli nie ma pracownikow w bazie 
            if (ListaPracownikow.Count == 0)
            {
                Console.WriteLine("Brak pracownikow w bazie!");
                return;
            }
            
            //  Uzywam paczke z NugetPackage - ,,ConsoleTables"
            var table = new ConsoleTable("Id", "Imie",
                "Nazwisko", "Wiek", "Stanowisko", 
                "Login" , "Data Zatrudnienia");
            foreach (Worker worker in ListaPracownikow)
            {
                var stanowisko = worker.IdRole == 1 ? "Admin" 
                    : (worker.IdRole == 2 ? "Rekruter" 
                    : ( worker.IdRole == 3 ? "Programista" 
                    : "Dzial HR"));

                table.AddRow(worker.Id, worker.Name,
                worker.Surname, worker.Age ,stanowisko,
                worker.Login, worker.HireDate);
            }

            table.Options.EnableCount = false; // Wylaczenie numeracji wierszy
            table.Write(); // Wyswietla tabele 
        }

        public void WyswietlPracownika(int id)
        {
            string query = $"SELECT * FROM workers WHERE id_worker = {id}";
            MySqlCommand cmd = new MySqlCommand(query, _conn);

            _conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            // Jesli pracownik o podanym ID istnieje
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
                    worker.Login = reader["login"].ToString();

                    ListaPracownikow.Add(worker); 
                }

                _conn.Close();

                //  Uzywam paczke z NugetPackage - ,,ConsoleTables"
                var table = new ConsoleTable("Id", "Imie",
                    "Nazwisko", "Wiek", "Stanowisko", 
                    "Login" , "Data Zatrudnienia");
                foreach (Worker worker in ListaPracownikow)
                {
                    var stanowisko = worker.IdRole == 1 ? "Admin"
                        : (worker.IdRole == 2 ? "Rekruter"
                        : (worker.IdRole == 3 ? "Programista"
                        : "Dzial HR"));

                    table.AddRow(worker.Id, worker.Name,
                        worker.Surname, worker.Age ,stanowisko,
                        worker.Login, worker.HireDate);
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
            string query = "SELECT role.role_name, COUNT(workers.id_role) as ilosc_pracownikow " +
                "FROM role " +
                "LEFT JOIN workers ON role.id_role = workers.id_role " +
                "GROUP BY role.role_name";
            MySqlCommand cmd = new MySqlCommand(query, _conn);

            _conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader["role_name"] + " - " + reader["ilosc_pracownikow"]);
            }

            _conn.Close();
        }

        public void DodajPracownika(Worker worker)
        {
            Console.WriteLine("Podaj imie (max 24 znaki): ");
            string nameInput = Console.ReadLine();
            worker.Name = nameInput.Substring(0, Math.Min(24, nameInput.Length));
            
            Console.WriteLine("Podaj nazwisko (max 80 znakow): ");
            string surnameInput = Console.ReadLine();
            worker.Surname = surnameInput.Substring(0, Math.Min(80, surnameInput.Length));
            
            Console.WriteLine("Podaj login (max 16 znakow): ");
            string loginInput = Console.ReadLine();
            worker.Login = loginInput.Substring(0, Math.Min(16, loginInput.Length));
            
            Console.WriteLine("Podaj haslo (max 30 znakow): ");
            string passwordInput = Console.ReadLine();
            worker.Password = passwordInput.Substring(0, Math.Min(30, passwordInput.Length));
            
            Console.WriteLine("Podaj wiek: ");
            worker.Age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Podaj id stanowiska: ");
            worker.IdRole = Convert.ToInt32(Console.ReadLine());
            
            // data zatrudnienia - pobiera terazniejsza date
            worker.HireDate = DateTime.Now;
            string hireDate = worker.HireDate.ToString("yyyy-MM-dd HH:mm:ss");
            
            worker.IsWorking = Convert.ToInt16(true);

            string query = $"INSERT INTO workers " +
                           $"(name, surname, login, password, age, id_role, hire_date, is_working) " +
                           $"VALUES ('{worker.Name}', '{worker.Surname}', '{worker.Login}'" +
                           $", '{worker.Password}', {worker.Age}, {worker.IdRole}," +
                           $" '{hireDate}', {worker.IsWorking})";
            MySqlCommand cmd = new MySqlCommand(query, _conn);

            _conn.Open();
            cmd.ExecuteNonQuery();
            _conn.Close();

            Console.WriteLine("Dodano nowego pracownika.");
        }

        public void UsunPracownika(int id)
        {
            string query = $"DELETE FROM workers WHERE id_worker = {id}";
            MySqlCommand cmd = new MySqlCommand(query, _conn);
            
            _conn.Open();
            // Usun pracownika, gdy istnieje

            if (cmd.ExecuteNonQuery() == 0)
            {
                Console.WriteLine("Nie ma pracownika o podanym ID.");
                return;
            }
            else
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine($"Usunieto pracownika o id {id}.");
            }
            
            
            _conn.Close();
        }
    }
}
