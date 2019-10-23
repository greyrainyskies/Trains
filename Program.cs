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
            Console.WriteLine("Haetaan asemia...");
            SearchLogic.PopulateStationDictionary();
            Console.WriteLine(SearchLogic.ConvertUserInputStationToShortCode("eno"));
            Console.WriteLine(SearchLogic.ConvertUserInputStationToShortCode("Seinäjoki"));
            Console.WriteLine(SearchLogic.ConvertUserInputStationToShortCode("Tampere"));


            var from = new Station();
            from.stationShortCode = "HKI";
            from.stationName = "Helsinki";
            var to = new Station();
            to.stationShortCode = "PSL";
            to.stationName = "Pasila";
            SearchLogic.SearchBetweenStations(from, to);
            SearchLogic.GetTrainRoute();

            SearchLogic.CurrentStationInfo(to);
        }
    }
}
