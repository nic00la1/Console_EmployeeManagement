using Console_EmployeeManagement.DB_Managament;
using Console_EmployeeManagement.Models;

namespace Console_EmployeeManagement.UI
{
    public class Utilities
    {
        public void WyswietlPracownikow()
        {
            Console.Clear();
            Console.WriteLine("Wszyscy pracownicy: \n");

            DatabaseManagament db = new DatabaseManagament();

            db.WyswietlPracownikow();
            Console.ReadKey(); // Dopoki nie nacisniesz klawisza program nie wyjdzie z metody
        }

        public void WyswietlPracownika()
        { 
            Console.Clear();
            Console.WriteLine("Podaj id pracownika: \n");

            DatabaseManagament db = new DatabaseManagament();

            int id = Convert.ToInt32(Console.ReadLine());

            db.WyswietlPracownika(id);
            Console.ReadKey(); // Dopoki nie nacisniesz klawisza program nie wyjdzie z metody
        }

        public void WyswietlStanowiska()
        {
            Console.Clear();
            Console.WriteLine("Stanowiska + liczba osob ktore na nich pracuje:\n");

            DatabaseManagament db = new DatabaseManagament();

            db.WyswietlStanowiska();
            Console.ReadKey(); // Dopoki nie nacisniesz klawisza program nie wyjdzie z metody
        }

        public void DodajPracownika()
        {
            Console.Clear();
            Console.WriteLine("Dodaj nowego pracownika: \n");

            DatabaseManagament db = new DatabaseManagament();
            Worker worker = new Worker();

            db.DodajPracownika(worker);
            Console.ReadKey(); // Dopoki nie nacisniesz klawisza program nie wyjdzie z metody
        }
    }
}
