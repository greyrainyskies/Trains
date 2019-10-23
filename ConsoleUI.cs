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


        public static void UserInput()
        {
            SearchLogic.PopulateStationDictionary();
            Console.WriteLine("Welcome to TrainSearch!");
            Console.WriteLine("");

            Console.Write("from:");

            string from = Console.ReadLine().ToUpper().Trim();

            Console.Write("to:");

            string to = Console.ReadLine().ToUpper().Trim();






            try
            {
                var fromStation = SearchLogic.ConvertUserInputStringToStation(from);
                var toStation = SearchLogic.ConvertUserInputStringToStation(to);
                SearchLogic.SearchBetweenStations(fromStation, toStation);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

            if (SearchLogic.stationDictionary.ContainsKey(to))

            {
                Console.WriteLine(SearchLogic.stationDictionary[to]);
            }


            Console.WriteLine("");

            //SearchLogic.SearchBetweenStations(from, to);
            SearchLogic.GetTrainRoute();


            //Console.Write("Search train:");
            //string trainSearch = Console.ReadLine();
            //Console.Write("Search station:");
            //string stationSearch = Console.ReadLine();



        }


    }
}
