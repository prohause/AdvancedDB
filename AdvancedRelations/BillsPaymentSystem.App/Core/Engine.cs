﻿using BillsPaymentSystem.App.Core.Contracts;
using BillsPaymentSystem.Data;
using System;

namespace BillsPaymentSystem.App.Core
{
    public class Engine : IEngine
    {
        private readonly ICommandInterpreter _commandInterpreter;

        public Engine(ICommandInterpreter commandInterpreter)
        {
            _commandInterpreter = commandInterpreter;
        }

        public void Run()
        {
            while (true)
            {
                var inputParams = Console.ReadLine()?.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                using (var context = new BillsPaymentSystemContext())
                {
                    var result = _commandInterpreter.Read(inputParams, context);
                    Console.WriteLine(result);
                }

                //TODO
            }
        }
    }
}