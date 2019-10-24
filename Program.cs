using System;
using RataDigiTraffic;
using RataDigiTraffic.Model;
using System.Collections.Generic;


namespace Trains
{
    class Program
    {
        static void Main(string[] args)
        {

            ConsoleUI.StartMenu();
            
              SearchLogic.PopulateStationDictionary();
            if (args.Length != 0)
            {
                CommandLineUI.RunFromCommandLine(args);
            }


        }
    }
}
