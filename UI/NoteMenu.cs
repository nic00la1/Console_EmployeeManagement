namespace Console_EmployeeManagement.UI;

public class NoteMenu
{
    static string[] notePositions =
    {
        "1. Dodaj notatke",
        "2. Usun notatke",
        "3. Wyswietl notatke",
        "4. Powrot do menu glownego"
    };
    static int activeNotePosition = 0;

    public static void StartNoteMenu()
    {
        while (true)
        {
            ShowNoteMenu();
            ChoosingNoteOption();
            RunNoteOption();
        }
    }

    private static void ShowNoteMenu()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(">>> Menu Notatek: <<<");
        Console.WriteLine();

        for (int i = 0; i < notePositions.Length; i++)
        {
            if (i == activeNotePosition)
            {
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("{0, -35}", notePositions[i]);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }
            else
            {
                Console.WriteLine(notePositions[i]);
            }
        }
    }

    private static void ChoosingNoteOption()
    {
        do
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.UpArrow)
            {
                activeNotePosition = activeNotePosition > 0 ? activeNotePosition - 1 : notePositions.Length - 1;
                ShowNoteMenu();
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                activeNotePosition = (activeNotePosition + 1) % notePositions.Length;
                ShowNoteMenu();
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                activeNotePosition = notePositions.Length - 1;
                break;
            }
            else if (key.Key == ConsoleKey.Enter)
                break;

        } while (true);
    }

    private static void RunNoteOption()
    {
        Utilities utilities = new Utilities();

        switch (activeNotePosition)
        {
            case 0:
                utilities.DodajNotatke();
                break;
            case 1:
                // utilities.UsunNotatke(); // Dodaj tę metodę do klasy Utilities
                break;
            case 2:
                // utilities.WyswietlNotatke(); // Dodaj tę metodę do klasy Utilities
                break;
            case 3:
                Menu.StartMenu();
                break;
        }
    }
}