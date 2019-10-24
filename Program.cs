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
            if (args.Length != 0)
            {
                CommandLineUI.RunFromCommandLine(args);
            }
            //ConsoleUI ui = new ConsoleUI();
            //ConsoleUI.StartMenu();




            //Console.WriteLine("Haetaan asemia...");
            //SearchLogic.PopulateStationDictionary();
            //Console.WriteLine(SearchLogic.ConvertUserInputStationToShortCode("eno"));


            //var from = new Station();
            //from.stationShortCode = "HKI";
            //from.stationName = "Helsinki";

            //var to = new Station();
            //to.stationShortCode = "PSL";
            //to.stationName = "Pasila";

            //SearchLogic.SearchBetweenStations(from, to);
            //SearchLogic.GetTrainRoute();

            //Console.WriteLine("Haetaan asemia...");
            //SearchLogic.PopulateStationDictionary();
            //Console.WriteLine(SearchLogic.ConvertUserInputStationToShortCode("eno"));
            //Console.WriteLine(SearchLogic.ConvertUserInputStationToShortCode("Seinäjoki"));
            //Console.WriteLine(SearchLogic.ConvertUserInputStationToShortCode("Tampere"));


            //var from = new Station();
            //from.stationShortCode = "HKI";
            //from.stationName = "Helsinki";
            //var to = new Station();
            //to.stationShortCode = "PSL";
            //to.stationName = "Pasila";
            //var stat = new Station();
            //stat.stationShortCode = "RKI";
            //stat.stationName = "Ruukki";
            //SearchLogic.SearchBetweenStations(from, to);
            //SearchLogic.GetTrainRoute();
            //SearchLogic.CurrentStationInfo(to);
            //var trainList = SearchLogic.CurrentStationInfoWithLimit(to);
            //SearchLogic.ShowUpcomingArrivals(to, trainList);
            //SearchLogic.ShowPastArrivals(to, trainList);
            //SearchLogic.ShowUpcomingDepartures(to, trainList);
            //SearchLogic.ShowPastDepartures(to, trainList);
            //var trainList2 = SearchLogic.CurrentStationInfoWithLimit(stat);
            //SearchLogic.ShowUpcomingArrivals(stat, trainList2);
            //SearchLogic.ShowPastArrivals(stat, trainList2);
            //SearchLogic.ShowUpcomingDepartures(stat, trainList2);
            //SearchLogic.ShowPastDepartures(stat, trainList2);
            //SearchLogic.SearchBetweenStations(from, to);

            //SearchLogic.GetTrainRoute();

            //Station testiasema = SearchLogic.stationDictionary["LAPPEENRANTA"];
            //Console.WriteLine("testiasema:" + testiasema.stationName);
            //Console.WriteLine("longitude " + testiasema.longitude);
            //Console.WriteLine("lat " + testiasema.latitude);

            //Console.WriteLine("distance from station: d" + SearchLogic.GetTrainDistanceFromStation(testiasema) + "km"); //junan etäisyys pasilan asemalta
            //Console.WriteLine("paluuarvo dist");
            //SearchLogic.GetTrainDistanceFromStation(testiasema);//junan etäisyys pasilan asemalta
        }
    }
}
