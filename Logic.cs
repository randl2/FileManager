namespace FileManager;

using System;
using System.IO;

class Logic
{
    private string currentDirectory;
    private int selectedIndex;

    public Logic()
    {
        currentDirectory = "c://";
        selectedIndex = 0;
    }

    public void Run()
    {
        bool isWorking = true;

        while (isWorking)
        {
            Console.Clear();
            Console.WriteLine("Папка: " + currentDirectory);
            Console.WriteLine("-------------------------------");
            ListFilesAndFolders();

            ConsoleKeyInfo keyPressed = Console.ReadKey();

            switch (keyPressed.Key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedIndex > 0)
                    {
                        selectedIndex--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (selectedIndex < Directory.GetFileSystemEntries(currentDirectory).Length - 1)
                    {
                        selectedIndex++;
                    }
                    break;
                case ConsoleKey.Enter:
                    string selectedItem = Directory.GetFileSystemEntries(currentDirectory)[selectedIndex];

                    if (File.Exists(selectedItem))
                    {
                        OpenTextFile(selectedItem);
                    }
                    else if (Directory.Exists(selectedItem))
                    {
                        try
                        {
                            currentDirectory = selectedItem;
                            selectedIndex = 0;
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Console.WriteLine("У вас немає доступу до цієї папки.");
                            Console.ReadLine();
                        }
                    }
                    break;
                case ConsoleKey.Escape:
                    if (currentDirectory != "c://")
                    {
                        try
                        {
                            currentDirectory = Directory.GetParent(currentDirectory).FullName;
                            selectedIndex = 0;
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Console.WriteLine("У вас немає доступу до цієї папки.");
                            Console.ReadLine();
                        }
                    }
                    break;
                case ConsoleKey.Q:
                    isWorking = false;
                    break;
            }
        }
    }

    private void ListFilesAndFolders()
    {
        try
        {
            string[] filesAndFolders = Directory.GetFileSystemEntries(currentDirectory);
            for (int i = 0; i < filesAndFolders.Length; i++)
            {
                string name = Path.GetFileName(filesAndFolders[i]);
                bool isDirectory = Directory.Exists(filesAndFolders[i]);

                if (selectedIndex == i)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("> ");
                }
                else
                {
                    Console.Write("  ");
                }

                if (isDirectory)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(name + " [Папка]");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(name + " [Файл]");
                }

                Console.ResetColor();
            }

            if (currentDirectory != "c://")
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Натиність Esc, щоб вийти з папки.");
            }

            Console.WriteLine("\nНатисніть Q, щоб завершити роботу.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("У вас немає доступу до цієї папки.");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Натисніть Esc, щоб повернутись назад до папки.");
        }
    }

    private void OpenTextFile(string filePath)
    {
        Console.Clear();
        Console.WriteLine("Файл \"" + filePath + "\":");
        Console.WriteLine("-------------------------------");

        try
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка з відкриттям файлу: " + ex.Message);
        }

        Console.WriteLine("-------------------------------");
        Console.WriteLine("Натисніть Enter, щоб повернутись назад до папки.");
        Console.ReadLine();
    }
}