using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

public static class CommandParser

{
    private  const char SINGLE_QUOTE = '\'';
    private  const char DOUBLE_QUOTE = '\"';
    private  const char ESCAPE = '\\';


    public static List<string> Parse(string target)
    {
        char prevChar = ' ';
        var curArgBuilder = new StringBuilder();
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

            if (s == DOUBLE_QUOTE)
            {
                StringBuilder doubleQuoteArgBuilder = new("");
                i++;
                while (i < target.Length && target[i] != DOUBLE_QUOTE)
                {
                    if (target[i] == ESCAPE && i + 1 < target.Length)
                    {
                        if (target[i + 1] == ESCAPE)
                        {
                            doubleQuoteArgBuilder.Append(ESCAPE);
                            i += 2;
                        }
                        else if (target[i + 1] == DOUBLE_QUOTE)
                        {
                            doubleQuoteArgBuilder.Append(DOUBLE_QUOTE);
                            i += 2;
                        }
                        else
                        {
                            doubleQuoteArgBuilder.Append(ESCAPE);
                            i++;
                        }
                        continue;
                    }

                    doubleQuoteArgBuilder.Append(target[i]);
                    i++;
                }
                curArgBuilder.Append(doubleQuoteArgBuilder.ToString());
                prevChar = DOUBLE_QUOTE;
                continue;
            }

            if (s == ESCAPE)
            {
                if (i + 1 < target.Length)
                {
                    i++;
                    curArgBuilder.Append(target[i]);
                    prevChar = target[i];
                    continue;
                }
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
