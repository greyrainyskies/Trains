using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Trains
{
    static class CommandLineUI
    {
        public static void RunFromCommandLine(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<BetweenOptions, RouteOptions, DistanceOptions>(args)
                .MapResult(
                    (BetweenOptions opts) => RunBetweenStations(opts),
                    (RouteOptions opts) => RunTrainRoute(opts),
                    (DistanceOptions opts) => RunTrainDistance(opts),

                    errs => 1);
        }

        static int RunBetweenStations(BetweenOptions opts)
        {
            var fromStation = SearchLogic.ConvertUserInputStringToStation(opts.FromStation);
            var toStation = SearchLogic.ConvertUserInputStringToStation(opts.ToStation);
            SearchLogic.SearchBetweenStations(fromStation, toStation);
            return 1;
        }

        static int RunTrainRoute(RouteOptions opts)
        {
            var trainNum = SearchLogic.GetTrainNumber(opts.TrainNumber);
            SearchLogic.GetTrainRoute(trainNum);
            return 1;
        }

        static int RunTrainDistance(DistanceOptions opts)
        {
            var trainNum = SearchLogic.GetTrainNumber(opts.TrainNumber);
            var station = SearchLogic.ConvertUserInputStringToStation(opts.Station);
            SearchLogic.GetTrainDistanceFromStation(station, trainNum);
            return 1;
        }
    }

    [Verb("between", HelpText = "Search for train connections between two stations.")]
    class BetweenOptions
    {
        [Option('f', "from", Required = true, HelpText = "The station from which trains are searched.")]
        public string FromStation { get; set; }

        [Option('t', "to", Required = true, HelpText = "The station to which trains are searched.")]
        public string ToStation { get; set; }
    }


    [Verb("route", HelpText = "Get the route for a specific train number.")]
    class RouteOptions
    {
        [Option('n', "train-number", Required = true, HelpText = "The number of the train. May be in the form 'IC47' or '47'.")]
        public string TrainNumber { get; set; }

    }

    [Verb("distance", HelpText = "Get the distance of a train from your station if the station is on the train's route and the train has not yet passed the station.")]
    class DistanceOptions
    {
        [Option('s', "station", Required = true, HelpText = "Station.")]
        public string Station { get; set; }

        [Option('n', "train-number", Required = true, HelpText = "The number of the train. May be in the form 'IC47' or '47'.")]
        public string TrainNumber { get; set; }

    }
}
