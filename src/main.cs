using System.Collections;
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

            if (command == "echo") {
                string[] args = parsedCommand[1..];

                echo(args);
                continue;
            }

            Console.WriteLine($"{input}: command not found");
        }

    }

    private static string[] parse(string command) {

        var parsedCommand = command.Split(" ");
        return parsedCommand;
    }

    private static void echo(string[] args) {
        var idx = 0;
        foreach ( var arg in args ) {
            if (idx == 0) {
                Console.Write(arg);
                idx++;
                continue;
            }

            Console.Write($" {arg}");
            idx++;
        }
        Console.WriteLine();
    }
}
