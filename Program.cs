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

            var oulu = new Station();
            oulu.stationShortCode = "HKI";
            var ruukki = new Station();
            ruukki.stationShortCode = "TKL";

            var tulos = SearchLogic.SearchBetweenStations(oulu, ruukki);

            foreach (var tieto in tulos)
            {
                Console.WriteLine(tieto);
            }
        }
    }
}
