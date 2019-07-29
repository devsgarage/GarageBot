using Microsoft.Extensions.DependencyInjection;
using Service.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GarageBot
{
    public static class BotHelper
    {
        public static void AddCommands(this IServiceCollection collection)
        {
            var type = typeof(IChatCommand);
            List<Type> types = new List<Type>();
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var di = new DirectoryInfo(path);
            var dlls = di.GetFiles("*.dll");
            foreach (var file in dlls)
            {
                try
                {
                    var nextAssembly = Assembly.LoadFrom(file.FullName);
                    types.AddRange(nextAssembly.GetTypes()
                        .Where(z => type.IsAssignableFrom(z) && !z.IsInterface && !z.IsAbstract)
                        .ToList());                    
                }
                catch (BadImageFormatException)
                {
                    // Not a .net assembly  - ignore
                }
            }

            foreach (var t in types)
            {
                //var chatCommand = (IChatCommand)Activator.CreateInstance(t);
                collection.AddSingleton(type, t);
                //logger.LogInformation($"Loaded command: {chatCommand.Command}");
                //Console.WriteLine($"Loaded command: {chatCommand.Command}");
            }
        }
    }
}
