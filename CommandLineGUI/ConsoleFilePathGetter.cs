using System;
using BusinessLogic.API;

namespace CommandLineGUI
{
    public class ConsoleFilePathGetter : IFilePathGetter
    {
        public string GetFilePath()
        {
            Console.Write("Provide file path: ");
            return Console.ReadLine();
        }
    }
}
