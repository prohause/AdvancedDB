using BillsPaymentSystem.App.Core;
using BillsPaymentSystem.App.Core.Contracts;
using BillsPaymentSystem.Data;

namespace BillsPaymentSystem.App
{
    internal class StartUp
    {
        private static void Main(string[] args)
        {
            using (var context = new BillsPaymentSystemContext())
            {
                //DbInitializer.Seed(context);

                ICommandInterpreter commandInterpreter = new CommandInterpreter();

                IEngine engine = new Engine(commandInterpreter);

                engine.Run();
            }
        }
    }
}