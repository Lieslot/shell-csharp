using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IOutputWriter
{
    void WriteLine(string message, bool isError);
    void Write(string message, bool isError);

}