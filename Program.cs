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
            SearchLogic.PopulateStationDictionary();
        }


        //printtaa asemien nimet testiksi
        //public static void GetStations()
        //{
        //    APIUtil utils = new APIUtil();
        //    List<Station> result = utils.Stations();

        //    foreach (var station in result)
        //    {
        //        Console.WriteLine(station.stationName);
        //    }

        //}
    }
}
