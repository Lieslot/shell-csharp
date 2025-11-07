using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

public class CommandDispatcher
{


    private readonly CommandController controller;
    private readonly Dictionary<string, MethodInfo> routeMap;
    private readonly MethodInfo? defaultRoute;

    public CommandDispatcher(CommandController controller)
    {
        this.controller = controller;
        this.routeMap = [];

        var methods = typeof(CommandController).GetMethods(BindingFlags.Public | BindingFlags.Instance);


        foreach (var method in methods)
        {
            var routeAttribute = method.GetCustomAttribute<CommandRoute>();

            if (routeAttribute != null)
            {
                if (routeAttribute.IsDefault)
                {
                    this.defaultRoute = method;
                }
                else if (!string.IsNullOrEmpty(routeAttribute.Command))
                {
                    this.routeMap[routeAttribute.Command] = method;
                }
            }
        }
    }

    public object? Dispatch(List<string> commands)
    {
        var command = commands[0];

        if (this.routeMap.TryGetValue(command, out var method))
        {
            return method.Invoke(controller, [commands]);
        }
        

        if (defaultRoute != null)
        {
            return defaultRoute.Invoke(controller, [commands]);
        }
        
        return null;
    }

}