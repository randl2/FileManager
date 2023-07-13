using System.Text;

namespace FileManager;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Logic logic = new Logic();

        logic.Run();
    }
}