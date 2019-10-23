using RataDigiTraffic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trains
{
     class ConsoleUI
    {
       
        public static void UserInputFromTo()
        {

            Console.Title = "UI";
            string title = @"
+-----------------------------------------------------+
|                                                     |
|        ▀▄▀▄▀▄▀▄▀▄▀▄ тraιnѕearcн ▄▀▄▀▄▀▄▀▄▀▄▀        |
|                                                     |
+-----------------------------------------------------+
|                                                     |
|                                                     |
|                                                     |
|    1. Search trains between start & end stations    |
|                                                     |
|    2. Information from specific train               |
|                                                     |
|    3. More Something                                |
|                                                     |
|    4. Still more to come                            |
|                                                     |
|                                                     |
|                                                     |
|                                                     |
|                                                     |
+-----------------------------------------------------+
";
            
            Console.WriteLine(title);
            ConsoleKeyInfo switchKey = Console.ReadKey();
            Console.Clear();
            switch (switchKey.Key)
            {
                case ConsoleKey.D1:
                    // Console.WriteLine("1");
                    var from = new Station();
                    from.stationShortCode = "HKI";
                    from.stationName = "Helsinki";
                    var to = new Station();
                    to.stationShortCode = "PSL";
                    to.stationName = "Pasila";
                    SearchLogic.SearchBetweenStations(from, to);
                    break;
                case ConsoleKey.D2:
                    SearchLogic.GetTrainRoute();
                    break;
                default:
                    throw new ArgumentException("Unhandled value: " + switchKey.ToString());
                  
            }

        }
    }
}
