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
            Console.WriteLine("Train route is: ");

            APIUtil api = new APIUtil();
            List<Train> TrainRoute = api.TrainRoute(9873); //Z-juna testaukseen

            foreach (var station in TrainRoute[0].timeTableRows)//index zero because there will only be one item in the list so no need to iterate through the "list"
            {
                if (station.commercialStop && station.type == "ARRIVAL") //if it's a station where the train stops
                {
                    string stationName = stationDictionary[station.stationShortCode].stationName;
                    Console.WriteLine(stationName + ", " + station.scheduledTime.ToString());
                }
            }
        }


        public static void SearchBetweenStations(Station from, Station to, int numberToPrint = 5)
        {
            var api = new APIUtil();
            var fromShortCode = from.stationShortCode;
            var toShortCode = to.stationShortCode;

            var trains = api.TrainsBetween(fromShortCode, toShortCode, numberToPrint);

            Console.WriteLine($"Next {trains.Count} " + (trains.Count > 0 ? "trains" : "train") + $" between {from.stationName} and {to.stationName}:");

            foreach (var t in trains)
            {
                var sb = new StringBuilder();
                sb.AppendLine(TrainName(t));
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

        public static void CurrentStationInfo(Station station, int minutesBeforeDeparture = 15, int minutesAfterDeparture = 15, int minutesBeforeArrival = 15, int minutesAfterArrival = 15)
        {
            var api = new APIUtil();
            var stationShortCode = station.stationShortCode;

            try
            {
                var trains = api.CurrentStationInfo(stationShortCode, minutesBeforeDeparture, minutesAfterDeparture, minutesBeforeArrival, minutesAfterArrival);

                var upcomingArrivalTimes = new List<(Train, TimetableRow)>();
                var pastArrivalTimes = new List<(Train, TimetableRow)>();
                var upcomingDepartureTimes = new List<(Train, TimetableRow)>();
                var pastDepartureTimes = new List<(Train, TimetableRow)>();

                foreach (var t in trains)
                {
                    var arrival = SearchForTimetableRow(stationShortCode, t.timeTableRows, TimetableRowType.Arrival);
                    if (arrival.Length != 0)
                    {
                        if (arrival[0].scheduledTime > DateTime.Now.ToUniversalTime() || arrival[0].liveEstimateTime > DateTime.Now.ToUniversalTime())
                        {
                            upcomingArrivalTimes.Add((t, arrival[0]));
                        }
                        else
                        {
                            pastArrivalTimes.Add((t, arrival[0]));
                        }
                    }
                    var departure = SearchForTimetableRow(stationShortCode, t.timeTableRows, TimetableRowType.Departure);
                    if (departure.Length != 0)
                    {
                        if (departure[0].scheduledTime > DateTime.Now.ToUniversalTime() || departure[0].liveEstimateTime > DateTime.Now)
                        {
                            upcomingDepartureTimes.Add((t, departure[0]));
                        } 
                        else
                        {
                            pastDepartureTimes.Add((t, departure[0]));
                        }
                    }
                }

                var sortUpcomingArrivals = from tuple in upcomingArrivalTimes
                                           orderby tuple.Item2.scheduledTime
                                           select tuple;

                upcomingArrivalTimes = sortUpcomingArrivals.ToList();

                var sortPastArrivals = from tuple in pastArrivalTimes
                                       orderby tuple.Item2.scheduledTime descending
                                       select tuple;

                pastArrivalTimes = sortPastArrivals.ToList();

                var sortUpcomingDepartures = from tuple in upcomingDepartureTimes
                                             orderby tuple.Item2.scheduledTime
                                             select tuple;

                upcomingDepartureTimes = sortUpcomingDepartures.ToList();

                var sortPastDepartures = from tuple in pastDepartureTimes
                                         orderby tuple.Item2.scheduledTime descending
                                         select tuple;

                pastDepartureTimes = sortPastDepartures.ToList();

                Console.WriteLine($"Current arrivals at {station.stationName}");
                foreach (var tuple in upcomingArrivalTimes)
                {
                    var train = tuple.Item1;
                    var sb = new StringBuilder();
                    var arrival = SearchForTimetableRow(stationShortCode, train.timeTableRows, TimetableRowType.Arrival);
                    if (arrival.Length != 0)
                    {
                        sb.Append(TrainName(train).PadRight(10));
                        var fromStation = stationDictionary[train.timeTableRows[0].stationShortCode].stationName;
                        sb.Append(fromStation.PadRight(16));
                        sb.Append(arrival[0].scheduledTime.ToLocalTime().ToLongTimeString().PadRight(7));
                        if (arrival[0].liveEstimateTime != DateTime.MinValue && arrival[0].liveEstimateTime != arrival[0].scheduledTime)
                        {
                            sb.Append("  =>  ");
                            sb.Append(arrival[0].liveEstimateTime.ToLocalTime().ToLongTimeString().PadRight(7));
                            sb.Append(" (");
                            var difference = arrival[0].differenceInMinutes;
                            sb.Append(difference < 1 && difference > -1 ? "< 1" : "~ " + Math.Abs(difference).ToString());
                            sb.Append(Math.Abs(difference) > 1 ? " minutes " : " minute ");
                            sb.Append(difference >= 0 ? "late)" : "early)");
                        }
                        Console.WriteLine(sb.ToString());
                    }
                }

                Console.WriteLine();
                Console.WriteLine($"Past arrivals at {station.stationName}");
                foreach (var tuple in pastArrivalTimes)
                {
                    var train = tuple.Item1;
                    var sb = new StringBuilder();
                    var arrival = SearchForTimetableRow(stationShortCode, train.timeTableRows, TimetableRowType.Arrival);
                    if (arrival.Length != 0)
                    {
                        sb.Append(TrainName(train).PadRight(10));
                        var fromStation = stationDictionary[train.timeTableRows[0].stationShortCode].stationName;
                        sb.Append(fromStation.PadRight(16));
                        sb.Append(arrival[0].scheduledTime.ToLocalTime().ToLongTimeString().PadRight(7));
                        if (arrival[0].actualTime != DateTime.MinValue && arrival[0].actualTime != arrival[0].scheduledTime)
                        {
                            sb.Append("  =>  ");
                            sb.Append(arrival[0].actualTime.ToLocalTime().ToLongTimeString().PadRight(7));
                            sb.Append(" (");
                            var difference = arrival[0].differenceInMinutes;
                            sb.Append(difference < 1 && difference > -1 ? "< 1" : "~ " + Math.Abs(difference).ToString());
                            sb.Append(Math.Abs(difference) > 1 ? " minutes " : " minute ");
                            sb.Append(difference >= 0 ? "late)" : "early)");
                        }
                        Console.WriteLine(sb.ToString());
                    }
                }

                Console.WriteLine();
                Console.WriteLine($"Current departures at {station.stationName}");
                foreach (var tuple in upcomingDepartureTimes)
                {
                    var train = tuple.Item1;
                    var sb = new StringBuilder();
                    var departure = SearchForTimetableRow(stationShortCode, train.timeTableRows, TimetableRowType.Departure);
                    if (departure.Length != 0)
                    {
                        sb.Append(TrainName(train).PadRight(10));
                        var toStation = stationDictionary[train.timeTableRows[train.timeTableRows.Count - 1].stationShortCode].stationName;
                        sb.Append(toStation.PadRight(16));
                        sb.Append(departure[0].scheduledTime.ToLocalTime().ToLongTimeString().PadRight(7));
                        if (departure[0].liveEstimateTime != DateTime.MinValue && departure[0].liveEstimateTime != departure[0].scheduledTime)
                        {
                            sb.Append("  =>  ");
                            sb.Append(departure[0].liveEstimateTime.ToLocalTime().ToLongTimeString().PadRight(7));
                            sb.Append(" (");
                            var difference = departure[0].differenceInMinutes;
                            sb.Append(difference < 1 && difference > -1 ? "< 1" : "~ " + Math.Abs(difference).ToString());
                            sb.Append(Math.Abs(difference) > 1 ? " minutes " : " minute ");
                            sb.Append(difference >= 0 ? "late)" : "early)");
                        }
                        Console.WriteLine(sb.ToString());
                    }
                }

                Console.WriteLine();
                Console.WriteLine($"Past departures at {station.stationName}");
                foreach (var tuple in pastDepartureTimes)
                {
                    var train = tuple.Item1;
                    var sb = new StringBuilder();
                    var departure = SearchForTimetableRow(stationShortCode, train.timeTableRows, TimetableRowType.Departure);
                    if (departure.Length != 0)
                    {
                        sb.Append(TrainName(train).PadRight(10));
                        var fromStation = stationDictionary[train.timeTableRows[train.timeTableRows.Count - 1].stationShortCode].stationName;
                        sb.Append(fromStation.PadRight(16));
                        sb.Append(departure[0].scheduledTime.ToLocalTime().ToLongTimeString().PadRight(7));
                        if (departure[0].actualTime != DateTime.MinValue && departure[0].actualTime != departure[0].scheduledTime)
                        {
                            sb.Append("  =>  ");
                            sb.Append(departure[0].actualTime.ToLocalTime().ToLongTimeString().PadRight(7));
                            sb.Append(" (");
                            var difference = departure[0].differenceInMinutes;
                            sb.Append(difference < 1 && difference > -1 ? "< 1" : "~ " + Math.Abs(difference).ToString());
                            sb.Append(Math.Abs(difference) > 1 ? " minutes " : " minute ");
                            sb.Append(difference >= 0 ? "late)" : "early)");
                        }
                        Console.WriteLine(sb.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }

        static TimetableRow[] SearchForTimetableRow(string shortCode, List<TimetableRow> rows, TimetableRowType rowType)
        {
            var query = from row in rows
                        where row.stationShortCode == shortCode && row.type == (rowType == TimetableRowType.Arrival ? "ARRIVAL" : "DEPARTURE")
                        select row;
            return query.ToArray();
        }

        static string TrainName(Train train)
        {
            return train.trainCategory == "Commuter" ? train.commuterLineID : train.trainType + " " + train.trainNumber;
        }
    }
}
