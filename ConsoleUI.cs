using System;
using System.Collections.Generic;
using System.Text;
using RataDigiTraffic.Model;
using RataDigiTraffic;

namespace Trains
{
    class ConsoleUI
    {
        

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
