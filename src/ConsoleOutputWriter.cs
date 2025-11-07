using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ConsoleOutputWriter : IOutputWriter
{
    public void WriteLine(string message, bool isError)
    {
        if (!isError)
        {
            Console.WriteLine(message);
        }

        else
        {
            Console.Error.WriteLine(message);
        }
        
    }

    public void Write(string message, bool isError)
    {
        if (!isError)
        {
            Console.Write(message);
        }

        else
        {
            Console.Error.Write(message);
        }
        
    }
    
}