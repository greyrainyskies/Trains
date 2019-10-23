using RataDigiTraffic;
using RataDigiTraffic.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Trains
{
    public enum TimetableRowType { Arrival, Departure };

    static class SearchLogic
    {
        static Dictionary<string, Station> stationDictionary = new Dictionary<string, Station>();

        //populates the stationDictionary with stationNames and stationShortCodes as keys and their respective station objects as values
        //needs to be run at the start-up of the app! (route method relies on this)
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

        //passes an int input (route number) to the api client and then prints the stations with arrival times
        public static void GetTrainRoute()
        {
            //parsing the input train number
            int trainNum = 0;
            bool format = false;
            while (!format)
            {
                Console.WriteLine("Enter a train number:");
                try
                {
                    string tempTrainNum = Console.ReadLine();
                    string numberOnly = Regex.Replace(tempTrainNum, "[^0-9.]", "");
                    trainNum = int.Parse(numberOnly);

                    format = true;
                }
                catch(FormatException)
                {
                    Console.WriteLine("Please enter a valid train number!");
                }
            }


            APIUtil api = new APIUtil();
            List<Train> TrainRoute = api.TrainRoute(trainNum); 
            if (TrainRoute.Count == 0)
            {
                Console.WriteLine("There are no trains operating with the given train number.");
                return;
            }
            //end of train number parsing

            Console.WriteLine($"The timetable for train {trainNum} is: ");
            Console.WriteLine();
            Console.WriteLine($"{"Station",-15}{"Time",10}{"Stop (minutes)", 20}");

            //initialisation of variables used in loop below
            DateTime stationArrivalTime = default;
            string shortStationArrivalTime = "";

            double stationStopDuration = 0;
            bool firstStation = true;
            string stationName = "";
            List<TimetableRow> station = TrainRoute[0].timeTableRows;

            for (int i=0; i < station.Count; i++)//index zero because there will only be one item in the list so no need to iterate through the "list"
            {
                if (station[i].commercialStop) //if it's a station where the train stops
                {
                    if (station[i].type == "ARRIVAL")
                    {
                        stationArrivalTime = station[i].scheduledTime;
                        shortStationArrivalTime = station[i].scheduledTime.ToLocalTime().ToString("HH:mm");
                        stationName = stationDictionary[station[i].stationShortCode].stationName;

                        if (i < station.Count-1 && station[i+1].type == "DEPARTURE")
                        {
                            stationStopDuration = (station[i+1].scheduledTime - stationArrivalTime).TotalMinutes;
                        }
                        Console.WriteLine($"{stationName,15}{shortStationArrivalTime,12}{(i==station.Count-1 ? "" : (stationStopDuration > 0 ? stationStopDuration.ToString() : "")),12}");
                    }

                    if (firstStation) //first station (when it doesn't have an arrival-pair)
                    {
                        stationName = stationDictionary[station[i].stationShortCode].stationName;
                        shortStationArrivalTime = station[i].scheduledTime.ToLocalTime().ToString("HH:mm");
                        firstStation = false;
                        Console.WriteLine($"{stationName,15}{shortStationArrivalTime,12}{(stationStopDuration > 0 ? stationStopDuration.ToString() : ""),12}");
                    }
                }
            }
        }


        public static void SearchBetweenStations(Station from, Station to, int numberToPrint = 5)
        {
            var api = new APIUtil();
            var fromShortCode = from.stationShortCode;
            var toShortCode = to.stationShortCode;

            var trains = api.TrainsBetween(fromShortCode, toShortCode, numberToPrint);

            Console.WriteLine($"Next {trains.Count} " + (trains.Count > 0 ? "trains" : "train" ) + $" between {from.stationName} and {to.stationName}:");

            foreach (var t in trains)
            {
                var sb = new StringBuilder();
                sb.AppendLine(t.trainCategory == "Commuter" ? "Commuter train " + t.commuterLineID : t.trainType + " " + t.trainNumber);
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
