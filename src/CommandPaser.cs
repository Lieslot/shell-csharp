using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class CommandParser

{
    private  const char SINGLE_QUOTE = '\'';
    private  const char DOUBLE_QUOTE = '\"';
 
    
    public static List<string> Parse(string target)
    {   
        char prevChar = ' ';
        StringBuilder curArgBuilder = new StringBuilder();
        var commands = new List<string>();

        for (var i = 0; i < target.Length; i++)
        {

            char s = target[i];

            if (s == SINGLE_QUOTE)
            {
                StringBuilder singleQuoteArgBuilder = new("");
                i++;
                while (i < target.Length && target[i] != SINGLE_QUOTE)
                {
                    singleQuoteArgBuilder.Append(target[i]);

                    i++;
                }
                curArgBuilder.Append(singleQuoteArgBuilder.ToString());
                prevChar = SINGLE_QUOTE;
                continue;
            }

            if (s == '\"')
            {
                StringBuilder singleQuoteArgBuilder = new("");
                i++;
                while (i < target.Length && target[i] != DOUBLE_QUOTE)
                {
                    singleQuoteArgBuilder.Append(target[i]);

                    i++;
                }
                curArgBuilder.Append(singleQuoteArgBuilder.ToString());
                prevChar = DOUBLE_QUOTE;
                continue;
            }


            var isEndOfCommand = prevChar != ' ' && s == ' ';

            if (s != ' ')
            {
                curArgBuilder.Append(s);
            }

            if (isEndOfCommand)
            {

                commands.Add(curArgBuilder.ToString());
                curArgBuilder.Clear();
            }

            prevChar = s;
        }

        if (curArgBuilder.Length != 0)
        {
            commands.Add(curArgBuilder.ToString());
        }

        return commands;
    }
}
