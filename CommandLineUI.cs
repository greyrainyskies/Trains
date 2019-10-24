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
            CommandLine.Parser.Default.ParseArguments<BetweenOptions>(args)
                .MapResult(
                    (BetweenOptions opts) => RunBetweenStations(opts),
                    errs => 1);
        }

        static int RunBetweenStations(BetweenOptions opts)
        {
            var fromStation = SearchLogic.ConvertUserInputStringToStation(opts.FromStation);
            var toStation = SearchLogic.ConvertUserInputStringToStation(opts.ToStation);
            SearchLogic.SearchBetweenStations(fromStation, toStation);
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
}
