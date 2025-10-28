using System.Data;

class Program
{
    static void Main()
    {

        while (true) {
            Console.Write("$ ");

            // ユーザーのinputを受け取る
            string input = Console.ReadLine();

            if (input == "") {
                Console.WriteLine($"{input}: command not found");        
                continue;
            }

            var parsedCommand = parse(input);
            var command = parsedCommand[0];

            if (command == "exit") {
                break;
            }

            Console.WriteLine($"{input}: command not found");
        }

    }

    private static string[] parse(string command) {

        var parsedCommand = command.Split(" ");

        return parsedCommand;
    }
}
