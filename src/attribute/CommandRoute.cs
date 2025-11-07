using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


[AttributeUsage(AttributeTargets.Method)]
public class CommandRoute : Attribute
{
    public string Command { get; set; }
    public bool IsDefault { get; set;  }

    public CommandRoute(string command = "", bool isDefault = false)
    {
        Command = command;
        IsDefault = isDefault;
    }

}