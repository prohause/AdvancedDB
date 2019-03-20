using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Contracts;
using System;
using System.Linq;

namespace MyApp.Core
{
    public class Engine : IEngine
    {
        private readonly IServiceProvider _provider;

        public Engine(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void Run()
        {
            while (true)
            {
                var inputArgs = Console.ReadLine()?
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();

                try
                {
                    var commandInterpreter = _provider.GetService<ICommandInterpreter>();
                    var result = commandInterpreter.Read(inputArgs);

                    Console.WriteLine(result);
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}