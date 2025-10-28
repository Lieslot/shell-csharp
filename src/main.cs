class Program
{
    static void Main()
    {

        while (true) {
            Console.Write("$ ");

            // ユーザーのinputを受け取る
            string input = Console.ReadLine();


            Console.WriteLine($"{input}: command not found");        
            
        }

    }
}
