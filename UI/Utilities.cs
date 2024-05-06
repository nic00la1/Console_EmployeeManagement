using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_EmployeeManagement.UI
{
    public class Utilities
    {
        public void WyswietlPracownikow()
        {
            Console.Clear();
            Console.WriteLine("Wszyscy pracownicy:");

            DatabaseManagement db = new DatabaseManagement();

            db.WyswietlPracownikow();
            Console.ReadKey(); // Dopoki nie nacisniesz klawisza program nie wyjdzie z metody
        }

        public void TymczasowaMetoda()
        {
            Console.WriteLine("Tymczasowa metoda");
            Console.ReadKey();
        }
    }
}
