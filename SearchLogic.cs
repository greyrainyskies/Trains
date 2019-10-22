using System;
using System.Collections.Generic;
using System.Text;
using RataDigiTraffic;
using RataDigiTraffic.Model;

namespace Trains
{
    static class SearchLogic
    {
        static Dictionary<string, Station> stationDictionary = new Dictionary<string, Station>();

        //populates the stationDictionary with stationNames and stationShortCodes as keys and their respective station objects as values
        public static void PopulateStationDictionary()
        {
            APIUtil utils = new APIUtil();

            List<Station> result = utils.Stations();

            foreach (var station in result)
            {
                stationDictionary.Add(station.stationName, station);
                //handles the case where Eno station has the short code eno
                //note: still contains "stations" with type "TURNOUT_IN_THE_OPEN_LANE"
                if (!stationDictionary.ContainsKey(station.stationShortCode))
                {
                    stationDictionary.Add(station.stationShortCode, station);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine();
                }
        }
            foreach (var item in stationDictionary)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }
        }

    }
}
