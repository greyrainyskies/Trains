using RataDigiTraffic.Model;
using System;

namespace Trains
{
    class Program
    {
        static void Main(string[] args)
        {
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
