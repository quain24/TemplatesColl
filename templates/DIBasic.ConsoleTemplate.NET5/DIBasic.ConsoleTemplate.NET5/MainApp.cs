using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DIBasic.ConsoleTemplate.NET5
{
    public class MainApp
    {
        private readonly ILogger<MainApp> _logger;

        public MainApp(ILogger<MainApp> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Main application execution method.
        /// </summary>
        /// <param name="args">Arguments from command line parameters</param>
        public async Task Run(string[] args)
        {
            Console.WriteLine("Welcome");
        }
    }
}