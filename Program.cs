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
<<<<<<< HEAD

            var oulu = new Station();
            oulu.stationShortCode = "HKI";
            var ruukki = new Station();
            ruukki.stationShortCode = "TKL";

            SearchLogic.SearchBetweenStations(oulu, ruukki);
=======
            var hki = new Station();
            hki.stationShortCode = "HKI";
            var tpe = new Station();
            tpe.stationShortCode = "TPE";

            SearchLogic.GetTrainRoute(SearchLogic.SearchTrainsBetweenStations(hki, tpe));
>>>>>>> urho
        }
    }
}
