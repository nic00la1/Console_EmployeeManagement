using Console_EmployeeManagement.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_EmployeeManagement.UI
{
    public class Menu
    {
        static string[] positions =
          {
                "1. Wyswietl pracownikow",
                "2. Wyswietl pracownika",
                "3. Wyswietl stanowiska",
                "4. Dodaj pracownika",
                "5. Usun pracownika",
                "6. Aktualizuj haslo pracownika",
                "7. Koniec"
          };
        static int activePosition = 0;

        public static void StartMenu()
        {
            Console.Title = "Zarzadzanie Pracownikami";
            Console.CursorVisible = false;
            while (true)
            {
                ShowMenu();
                ChoosingOption();
                RunOption();
            }
        }

        private static void ShowMenu()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(">>> Menu: <<<");
            Console.WriteLine();

            for (int i = 0; i < positions.Length; i++) // dla kazdej pozycji menu
            {
                if (i == activePosition) // jesli jest aktywna
                {
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("{0, -35}", positions[i]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                }
                else
                {
                    Console.WriteLine(positions[i]);
                }
            }
        }

        private static void ChoosingOption()
        {
            do
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    activePosition = activePosition > 0 ? activePosition - 1 : positions.Length - 1;
                    ShowMenu();
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    activePosition = (activePosition + 1) % positions.Length;
                    ShowMenu();
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    activePosition = positions.Length - 1;
                    break;
                }
                else if (key.Key == ConsoleKey.Enter)
                    break;

            } while (true);
        }

        private static void RunOption()
        {
            Utilities utilities = new Utilities();

            switch (activePosition)
            {
                case 0:
                    utilities.WyswietlPracownikow();
                    break;
                case 1:
                    utilities.WyswietlPracownika();
                    break;
                case 2:
                    utilities.WyswietlStanowiska();
                    break;
                case 3:
                    utilities.DodajPracownika();
                    break;
                case 4:
                    utilities.UsunPracownika();
                    break;
                case 5:
                    utilities.AktualizujHasloPracownika();
                    break;
                case 6:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}