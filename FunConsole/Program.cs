using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;

namespace FunConsole
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args[0] == "UpdateDatabase")
            {
                Console.WriteLine("Wiping and restoring database");

                // this is the class with tests listed above
                var databaseRestorer = new DatabaseSetup();

                try
                {
                    databaseRestorer.Wipe_And_Create_Database();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Failed to wipe and restore database");
                    Console.WriteLine(exception.ToString());
                    return 1;
                }

                Console.WriteLine("Restoring database complete");
            }
            else
            {
                Console.WriteLine(@"Nothing is happening. The only available command is UpdateDatabase. Use this program like this: ""C:/>MyApp.Tests.exe UpdateDatabse""");
            }

            return 0;
        }
    }
}
