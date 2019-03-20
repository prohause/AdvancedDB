using MyApp.Core.Commands.Contracts;
using MyApp.Core.Contracts;
using System;
using System.Linq;
using System.Reflection;

namespace MyApp.Core
{
    public class CommandInterpreter : ICommandInterpreter
    {
        private const string Suffix = "Command";
        private readonly IServiceProvider _serviceProvider;

        public CommandInterpreter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public string Read(string[] inputArgs)
        {
            var command = inputArgs[0] + Suffix;
            var commandArgs = inputArgs.Skip(1).ToArray();

            var type = Assembly.GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(x => x.Name == command);

            if (type == null)
            {
                throw new ArgumentNullException(command, "Command not found!");
            }

            var constructor = type.GetConstructors()
                .FirstOrDefault();

            if (constructor == null)
            {
                throw new ArgumentNullException(nameof(constructor), "No constructor found");
            }

            var constructorParams = constructor
                .GetParameters()
                .Select(x => x.ParameterType)
                .ToArray();

            var services = constructorParams
                .Select(_serviceProvider.GetService)
                .ToArray();

            var invokeConstructor = (ICommand)constructor.Invoke(services);

            var result = invokeConstructor.Execute(commandArgs);

            return result;
        }
    }
}