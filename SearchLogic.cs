using RataDigiTraffic;
using RataDigiTraffic.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Trains
{
    public enum TimetableRowType { Arrival, Departure };

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
                stationDictionary.Add(station.stationName.ToUpper(), station);
                //handles the case where Eno station has the short code eno
                //note: still contains "stations" with type "TURNOUT_IN_THE_OPEN_LANE"
                if (!stationDictionary.ContainsKey(station.stationShortCode))
                {
                    stationDictionary.Add(station.stationShortCode, station);
                }
            }
        }

        public static string ConvertUserInputStationToShortCode(string input)
        {
            Station userStation = stationDictionary[input.ToUpper().Trim()];
            string shortcode = userStation.stationShortCode;
            return shortcode;
        }

        public static List<string> SearchBetweenStations(Station from,  Station to)
        {
            var api = new APIUtil();
            var fromShortCode = from.stationShortCode;
            var toShortCode = to.stationShortCode;

            var trains = api.TrainsBetween(fromShortCode, toShortCode);

            var schedules = new List<string>();

            foreach (var t in trains)
            {
                string trainName = t.trainCategory == "Commuter" ? t.commuterLineID : t.trainType + t.trainNumber;
                var departure = SearchForTimetableRow(fromShortCode, t.timeTableRows, TimetableRowType.Departure)[0];
                var arrival = SearchForTimetableRow(toShortCode, t.timeTableRows, TimetableRowType.Arrival)[0];
                schedules.Add($"{trainName}, lähtee {departure.scheduledTime.ToString()}, saapuu {arrival.scheduledTime.ToString()}");
            }

            return schedules;
        }

        static TimetableRow[] SearchForTimetableRow(string shortCode, List<TimetableRow> rows, TimetableRowType rowType)
        {
            var query = from row in rows
                        where row.stationShortCode == shortCode && row.type == (rowType == TimetableRowType.Arrival ? "ARRIVAL" : "DEPARTURE")
                        select row;
            return query.ToArray();
        }
    }
}
