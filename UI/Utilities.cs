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
            Console.WriteLine("Podaj id pracownika, ktorego chcesz wyswietlic: \n");

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

        public void UsunPracownika()
        {
            Console.Clear();
            Console.WriteLine("Podaj id pracownika, ktorego chcesz usunac: \n");

            DatabaseManagament db = new DatabaseManagament();

            int id = Convert.ToInt32(Console.ReadLine());

            db.UsunPracownika(id);
            Console.ReadKey(); // Dopoki nie nacisniesz klawisza program nie wyjdzie z metody
        }

        public void AktualizujHasloPracownika()
        {
         Console.Clear();
         Console.WriteLine("Aktualizuj haslo pracownika: \n");
         
         DatabaseManagament db = new DatabaseManagament();
         
         Console.WriteLine("Podaj id pracownika: ");
         string input = Console.ReadLine();
         int id;
         
         if (!int.TryParse(input, out id))
         {
             Console.WriteLine("Invalid input. Please enter a valid ID.");
             return;
         }
         
         db.AktualizujHasloUsera(id);
         Console.ReadKey();
        }

        public void DodajNotatke()
        {
            Console.Clear();
            Console.WriteLine("Dodaj notatke: \n");
            
            DatabaseManagament db = new DatabaseManagament();
            Note note = new Note();
            
            db.DodajNotatke(note);
            Console.ReadKey();
        }
        
        public void UsunNotatke()
        {
            Console.Clear();
            Console.WriteLine("Podaj id notatki, ktora chcesz usunac: \n");
            
            DatabaseManagament db = new DatabaseManagament();
            
            int id = Convert.ToInt32(Console.ReadLine());
            
            db.UsunNotatke(id);
            Console.ReadKey();
        }

        public void WyswietlNotatki()
        {
            Console.Clear();
            Console.WriteLine("Podaj id pracownika, ktorego notatki chcesz wyswietlic: \n");

            int id = Convert.ToInt32(Console.ReadLine());
            
            DatabaseManagament db = new DatabaseManagament();
            
            db.WyswietlNotatki(id);
            Console.ReadKey();
        }
    }
}
