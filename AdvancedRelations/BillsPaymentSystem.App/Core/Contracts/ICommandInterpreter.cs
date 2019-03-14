using BillsPaymentSystem.Data;

namespace BillsPaymentSystem.App.Core.Contracts
{
    public interface ICommandInterpreter
    {
        string Read(string[] args, BillsPaymentSystemContext context);
    }
}