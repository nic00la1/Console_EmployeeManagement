using System.Text;
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
                "Nazwisko", "Stanowisko");
            foreach (Worker worker in ListaPracownikow)
            {
                var stanowisko = worker.IdRole == 1 ? "Admin" 
                    : (worker.IdRole == 2 ? "Rekruter" 
                    : ( worker.IdRole == 3 ? "Programista" 
                    : "Dzial HR"));

                table.AddRow(worker.Id, worker.Name,
                worker.Surname
                ,stanowisko);
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
            
            Console.WriteLine("Podaj date zatrudnienia (yyyy-MM-dd HH:mm:ss): ");
            worker.HireDate = Convert.ToDateTime(Console.ReadLine());
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
        
        // Opcja aktualizacji hasla pracownika,
        // ktore jest generowane automatycznie przez system
        public void AktualizujHasloUsera(int id)
        {
            // Generate a new password
            string newPassword = GenerujHaslo();

            string query = $"UPDATE workers SET password = '{newPassword}' WHERE id_worker = {id}";
            MySqlCommand cmd = new MySqlCommand(query, _conn);

            _conn.Open();
            int affectedRows = cmd.ExecuteNonQuery();
            if (affectedRows == 0)
            {
                Console.WriteLine("Nie ma pracownika o podanym ID.");
            }
            else
            {
                Console.WriteLine($"\nNowe haslo: {newPassword} --> |WERSJA TESTOWA|\n");
                
                Console.WriteLine($"Zaktualizowano haslo pracownika o id {id}.");
            }

            _conn.Close();
        }
        
        private string GenerujHaslo()
        {
            // Generuj losowe haslo w przedziale od 8 do 30 znakow 
            Random rnd = new Random();
            int length = rnd.Next(9, 31);
            
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            const string specialChars = "!@#$%^&*()";
            StringBuilder res = new StringBuilder();
            
            // Generuj losowe znaki alfanumeryczne ( czyli litery i cyfry) 
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            
            // Generuj losowe znaki specjalne i wstaw je w losowe miejsca
            int pos1 = rnd.Next(0, res.Length);
            int pos2;
            do
            {
                pos2 = rnd.Next(0, res.Length);
            } while (pos2 == pos1); // Upewnij sie, ze oba znaki nie sa na tych samych pozycjach

            // Znaki specjalne
            res.Insert(pos1, specialChars[rnd.Next(specialChars.Length)]);
            res.Insert(pos2, specialChars[rnd.Next(specialChars.Length)]);

            return res.ToString();
        }
        
        // === NOTATKI PRACOWNIKA === - opcja dodatkowa
        public void DodajNotatke(Note note)
        {
            Console.WriteLine("Podaj id pracownika: ");
            note.IdWorker = Convert.ToInt32(Console.ReadLine());

          string queryTest = $"SELECT * FROM workers WHERE id_worker = {note.IdWorker}";  
            MySqlCommand cmdTest = new MySqlCommand(queryTest, _conn);
            _conn.Open();
            
            MySqlDataReader reader = cmdTest.ExecuteReader();
            
            if (!reader.HasRows)
            {
                Console.WriteLine("Nie ma pracownika o podanym ID.");
                _conn.Close();
                return;
            }
            
            _conn.Close();
            
            Console.WriteLine("Podaj tresc notatki: ");
            note.Content = Console.ReadLine();
            note.AddedAt = DateTime.Now;
            string addedAd = note.AddedAt.ToString("yyyy-MM-dd HH:mm:ss");
            
            string query = $"INSERT INTO note (id_worker, content, added_at) " +
                           $"VALUES ({note.IdWorker}, '{note.Content}', '{addedAd}')";
            MySqlCommand cmd = new MySqlCommand(query, _conn);

            _conn.Open();
            cmd.ExecuteNonQuery();
            _conn.Close();

            Console.WriteLine("\nDodano notatke!");
        }
        
        public void UsunNotatke(int id)
        {
            string query = $"DELETE FROM note WHERE id_note = {id}";
            MySqlCommand cmd = new MySqlCommand(query, _conn);
            
            _conn.Open();
            // Usun notatke, gdy istnieje

            if (cmd.ExecuteNonQuery() == 0)
            {
                Console.WriteLine("Nie ma notatki o podanym ID.");
                return;
            }
            else
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine($"Usunieto notatke o id {id}.");
            }
            _conn.Close();
        }

        public void WyswietlNotatki(int id)
        {
            string query = $"SELECT * FROM note WHERE id_worker = {id}";
            MySqlCommand cmd = new MySqlCommand(query, _conn);
            
            _conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            
            if (!reader.HasRows)
            {
                Console.WriteLine("Brak notatek dla pracownika o podanym ID.");
                _conn.Close();
                return;
            }
            
            List<Note> notes = new List<Note>();
            
            while (reader.Read())
            {
                Note note = new Note();
                note.IdNote = Convert.ToInt32(reader["id_note"]);
                note.IdWorker = Convert.ToInt32(reader["id_worker"]);
                note.Content = reader["content"].ToString();
                note.AddedAt = Convert.ToDateTime(reader["added_at"]);
                
                notes.Add(note);
            }
            
            _conn.Close();
            
            var table = new ConsoleTable("Id", "Id Pracownika", "Tresc", "Data Dodania");
            foreach (Note note in notes)
            {
                table.AddRow(note.IdNote, note.IdWorker, note.Content, note.AddedAt);
            }
            
            table.Options.EnableCount = false; // Wylaczenie numeracji wierszy
            table.Write(); // Wyswietla tabele
        }
    }
}
