using RataDigiTraffic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trains
{
    class ConsoleUI
    {

        public static void StartMenu()
        {

            bool repeat = true;

            while (repeat)
            {
                Console.WriteLine("");
                Console.Title = "UI";
                string title = @"

                                                   
|        ▀▄▀▄▀▄▀▄▀▄▀▄ TrainSearch ▄▀▄▀▄▀▄▀▄▀▄▀        |
|                                                     |
+-----------------------------------------------------+
|                                                     |
|                                                     |
|                                                     |
|    1. Search trains between start & end stations    |
|                                                     |
|    2. Information from specific train route         |
|                                                     |
|    3. Show train distance from specific station     |
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
                        UserInput();
                        break;
                    case ConsoleKey.NumPad1:
                        UserInput();
                        break;
                    case ConsoleKey.D2:
                        SearchLogic.GetTrainRoute();
                        break;
                    case ConsoleKey.NumPad2:
                        SearchLogic.GetTrainRoute();
                        break;
                    case ConsoleKey.D3:
                        UserInputStation();
                        break;
                    case ConsoleKey.NumPad3:
                        UserInputStation();
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        // repeat = false;
                        break;

                    default:
                        throw new ArgumentException("Unhandled value: " + switchKey.ToString());

                }
                Console.WriteLine("Press ESC to go back to main menu");
                Console.ReadKey();
                Console.Clear();

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

            Console.WriteLine("");

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

        }

        public static void UserInputStation()
        {
            SearchLogic.PopulateStationDictionary();
            Console.WriteLine("Welcome to TrainSearch!");
            Console.WriteLine("");

            Console.Write("Select station:");

            string station = Console.ReadLine().ToUpper().Trim();

            try
            {
                var fromStation = SearchLogic.ConvertUserInputStringToStation(station);
                SearchLogic.GetTrainDistanceFromStation(fromStation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (SearchLogic.stationDictionary.ContainsKey(station))

            {
                //Console.WriteLine(SearchLogic.GetTrainDistanceFromStation(station);
            }


            Console.WriteLine("");




        }
    }
}
