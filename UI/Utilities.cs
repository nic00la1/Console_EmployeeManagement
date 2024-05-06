using Console_EmployeeManagement.DB_Managament;

namespace Console_EmployeeManagement.UI
{
    public class Utilities
    {
        public void WyswietlPracownikow()
        {
            Console.Clear();
            Console.WriteLine("Wszyscy pracownicy:");

            DatabaseManagament db = new DatabaseManagament();

            db.WyswietlPracownikow();
            Console.ReadKey(); // Dopoki nie nacisniesz klawisza program nie wyjdzie z metody
        }

        public void WyswietlPracownika()
        { 
            Console.Clear();
            Console.WriteLine("Podaj id pracownika: ");

            DatabaseManagament db = new DatabaseManagament();

            int id = Convert.ToInt32(Console.ReadLine());

            db.WyswietlPracownika(id);
            Console.ReadKey(); // Dopoki nie nacisniesz klawisza program nie wyjdzie z metody
        }

        public void TymczasowaMetoda()
        {
            Console.WriteLine("Tymczasowa metoda");
            Console.ReadKey();
        }
    }
}
