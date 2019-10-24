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

            SearchLogic.PopulateStationDictionary();
            bool repeat = true;

            while (repeat)
            {


                Console.Title = "UI";
                Console.WriteLine("");
                Console.WriteLine("");
                string title = @"
                

++                                                    |
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
|    3. Get train distance from station               |
|                                                     |
|    4. Station's next 5 arrivals / departures        |
|                                                     |
|    5. Station's arrivals / departures               |
|       ïn +- 15min.                                  |
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
                        var trainNumber = SearchLogic.GetTrainNumber();
                        SearchLogic.GetTrainRoute(trainNumber);
                        break;
                    case ConsoleKey.NumPad2:
                        var trainNumber02 = SearchLogic.GetTrainNumber();
                        SearchLogic.GetTrainRoute(trainNumber02);
                        break;
                    case ConsoleKey.D3:
                        UserInputStation(); // asemalta matkaa liikkuvaan junaan
                        break;
                    case ConsoleKey.NumPad3:
                        UserInputStation();
                        break;
                    case ConsoleKey.D4:
                        UserInputStationInfoWithLimit();
                        break;
                    case ConsoleKey.NumPad4:
                        UserInputStationInfoWithLimit();
                        break;
                    case ConsoleKey.D5:
                        UserInputStationInfoWithTime();
                        break;
                    case ConsoleKey.NumPad5:
                        UserInputStationInfoWithTime();
                        break;


                    case ConsoleKey.Escape:
                        Environment.Exit(0);
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

            Console.WriteLine("Welcome to TrainSearch!");
            Console.WriteLine("");

            Console.Write("Select station:");

            string station = Console.ReadLine().ToUpper().Trim();

            try
            {
                var distfromStation = SearchLogic.ConvertUserInputStringToStation(station);
                var trainNumber = SearchLogic.GetTrainNumber();
                SearchLogic.GetTrainDistanceFromStation(distfromStation, trainNumber);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            Console.WriteLine("");


        }
        public static void UserInputStationInfoWithLimit()
        {

            Console.WriteLine("Welcome to TrainSearch!");
            Console.WriteLine("");

            Console.Write("Select station:");

            string station = Console.ReadLine().ToUpper().Trim();

            try
            {
                var stationLocation = SearchLogic.ConvertUserInputStringToStation(station);
                var list = SearchLogic.CurrentStationInfoWithLimit(stationLocation);
                SearchLogic.ShowUpcomingArrivals(stationLocation, list);
                SearchLogic.ShowPastArrivals(stationLocation, list);
                SearchLogic.ShowUpcomingDepartures(stationLocation, list);
                SearchLogic.ShowUpcomingArrivals(stationLocation, list);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("");




        }
        public static void UserInputStationInfoWithTime()
        {

            Console.WriteLine("Welcome to TrainSearch!");
            Console.WriteLine("");

            Console.Write("Select station:");

            string station = Console.ReadLine().ToUpper().Trim();

            try
            {
                var stationLocation = SearchLogic.ConvertUserInputStringToStation(station);
                var list = SearchLogic.CurrentStationInfoWithTime(stationLocation);
                SearchLogic.ShowUpcomingArrivals(stationLocation, list);
                SearchLogic.ShowPastArrivals(stationLocation, list);
                SearchLogic.ShowUpcomingDepartures(stationLocation, list);
                SearchLogic.ShowUpcomingArrivals(stationLocation, list);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("");


        }
    }
}