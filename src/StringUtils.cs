using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class StringUtils
{
    
    public static string RemoveSingleQuotes(string target)
    {
        return target.Trim('\'');
    }
}