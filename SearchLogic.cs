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


        //gets trains between two stations specified by the user
        public static List<Train> SearchTrainsBetweenStations(Station from, Station to)
        {
            var api = new APIUtil();
            var fromShortCode = from.stationShortCode;
            var toShortCode = to.stationShortCode;

            var trains = api.TrainsBetween(fromShortCode, toShortCode);
            return trains;
        }

        public static void GetTrainRoute(List<Train> trainList)
        {
            Console.WriteLine("trains operating in this route: ");
            List<string> uniqueTrains = new List<string>();

            //var trainNumberQuery = from train in trainList
            //                       where train.commuterLineID != ""
            //                       select train.commuterLineID;
            //foreach (var item in trainNumberQuery)
            //{
            //    Console.WriteLine(item);
            //}

            foreach (var train in trainList)
            {
                if (train.commuterLineID == "")
                {
                    string nonCommuter = train.trainType + train.trainNumber.ToString();
                    if (!uniqueTrains.Contains(nonCommuter)){
                        uniqueTrains.Add(nonCommuter);
                    }
                }
                else
                {
                    string commuter = train.commuterLineID;
                    if (!uniqueTrains.Contains(commuter))
                    {
                        uniqueTrains.Add(commuter);
                    }
                }
            }

            foreach (var route in uniqueTrains)
            {
                Console.WriteLine(route);
            }
        }



        public static void SearchBetweenStations(Station from, Station to, int numberToPrint = 5)
        {
            var api = new APIUtil();
            var fromShortCode = from.stationShortCode;
            var toShortCode = to.stationShortCode;

            var trains = api.TrainsBetween(fromShortCode, toShortCode);

            if (trains.Count > numberToPrint)
            {
                trains = trains.Take(numberToPrint).ToList();
            }

            Console.WriteLine($"Next {trains.Count} " + (trains.Count > 0 ? "trains" : "train" ) + $" between {from.stationName} and {to.stationName}:");

            foreach (var t in trains)
            {
                var sb = new StringBuilder();
                sb.AppendLine(t.trainCategory == "Commuter" ? t.commuterLineID : t.trainType + " " + t.trainNumber);
                var departure = SearchForTimetableRow(fromShortCode, t.timeTableRows, TimetableRowType.Departure)[0];
                sb.AppendLine($"\tScheduled departure time from {from.stationName}: {departure.scheduledTime.ToLocalTime()}");
                if (departure.liveEstimateTime != DateTime.MinValue)
                {
                    sb.AppendLine($"\tEstimated departure time: {departure.liveEstimateTime.ToLocalTime()} ({departure.differenceInMinutes} minutes late");
                }
                var arrival = SearchForTimetableRow(toShortCode, t.timeTableRows, TimetableRowType.Arrival)[0];
                sb.AppendLine($"\tScheduled arrival time to {to.stationName}: {arrival.scheduledTime.ToLocalTime()}");
                if (arrival.liveEstimateTime != DateTime.MinValue)
                {
                    sb.AppendLine($"\tEstimated departure time: {arrival.liveEstimateTime.ToLocalTime()} ({arrival.differenceInMinutes} minutes late");
                }
                Console.WriteLine(sb.ToString());
            }
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
