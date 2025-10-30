using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class StringUtils
{
    
    public static string RemoveQuotes(string target)
    {
        return target.Trim('\'').Trim('\"');
    }
}