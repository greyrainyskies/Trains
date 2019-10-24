using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
using RataDigiTraffic.Model;

namespace Trains
{
    static class CommandLineUI
    {
        public static void RunFromCommandLine(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<BetweenOptions, CurrentStationInfoOptions, NextStationInfoOptions>(args)
                .MapResult(
                    (BetweenOptions opts) => RunBetweenStations(opts),
                    (CurrentStationInfoOptions opts) => RunCurrentStationInfo(opts),
                    (NextStationInfoOptions opts) => RunNextStationInfo(opts),
                    errs => 1);
        }

        static int RunBetweenStations(BetweenOptions opts)
        {
            var fromStation = SearchLogic.ConvertUserInputStringToStation(opts.FromStation);
            var toStation = SearchLogic.ConvertUserInputStringToStation(opts.ToStation);
            var limit = opts.Limit;
            if (limit != 0)
            {
                SearchLogic.SearchBetweenStations(fromStation, toStation, limit);
            }
            else
            {
                SearchLogic.SearchBetweenStations(fromStation, toStation);
            }

            return 1;
        }

        static int RunCurrentStationInfo(CurrentStationInfoOptions opts)
        {
            var station = SearchLogic.ConvertUserInputStringToStation(opts.Station);
            var limit = opts.Limit;
            var trains = new List<Train>();
            if (limit != 0)
            {
                trains = SearchLogic.CurrentStationInfoWithLimit(station, limit);
            }
            else
            {
                trains = SearchLogic.CurrentStationInfoWithLimit(station);
            }
            ShowStationData(station, trains, opts.showPast);
            return 1;
        }

        static int RunNextStationInfo(NextStationInfoOptions opts)
        {
            var station = SearchLogic.ConvertUserInputStringToStation(opts.Station);
            var minutes = opts.Minutes;
            var trains = new List<Train>();
            if (minutes != 0)
            {
                trains = SearchLogic.CurrentStationInfoWithTime(station, minutes, minutes, minutes, minutes);
            }
            else
            {
                trains = SearchLogic.CurrentStationInfoWithTime(station);
            }
            ShowStationData(station, trains, opts.showPast);
            return 1;
        }

        static void ShowStationData(Station station, List<Train> trains, bool showPast)
        {
            SearchLogic.ShowUpcomingDepartures(station, trains);
            if (showPast)
            {
                SearchLogic.ShowPastDepartures(station, trains);
            }
            SearchLogic.ShowUpcomingArrivals(station, trains);
            if (showPast)
            {
                SearchLogic.ShowPastArrivals(station, trains);
            }
        }
    }

    [Verb("between", HelpText = "Search for train connections between two stations.")]
    class BetweenOptions
    {
        [Option('f', "from", Required = true, HelpText = "The station from which trains are searched.")]
        public string FromStation { get; set; }

        [Option('t', "to", Required = true, HelpText = "The station to which trains are searched.")]
        public string ToStation { get; set; }

        [Option('l', "limit", Required = false, HelpText = "Parameter to limit the number of results. Default is 5.")]
        public int Limit { get; set; }
    }

    [Verb("station", HelpText = "Show current info at a specific station.")]
    class CurrentStationInfoOptions
    {
        [Option('s', "station", Required = true, HelpText = "Name of the station whose information is shown.")]
        public string Station { get; set; }

        [Option('l', "limit", Required = false, HelpText = "How many results are shown. Default 5.")]
        public int Limit { get; set; }

        [Option('p', "past", Required = false, HelpText = "Show also past departures and arrivals. ")]
        public bool showPast { get; set; }
    }

    [Verb("next", HelpText = "Show the next train information at a specific station.")]
    class NextStationInfoOptions
    {
        [Option('s', "station", Required = true, HelpText = "Name of the station whose information is shown.")]
        public string Station { get; set; }

        [Option('m', "minutes", Required = false, HelpText = "Trains shown for the next X minutes. Default 15 minutes.")]
        public int Minutes { get; set; }

        [Option('p', "past", Required = false, HelpText = "Show also past departures and arrivals.")]
        public bool showPast { get; set; }
    }

}
