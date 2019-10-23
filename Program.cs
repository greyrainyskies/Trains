﻿using System;
using RataDigiTraffic;
using RataDigiTraffic.Model;
using System.Collections.Generic;


namespace Trains
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Haetaan asemia...");
            SearchLogic.PopulateStationDictionary();
            Console.WriteLine(SearchLogic.ConvertUserInputStationToShortCode("eno"));


            var from = new Station();
            from.stationShortCode = "HKI";
            from.stationName = "Helsinki";
            var to = new Station();
            to.stationShortCode = "PSL";
            to.stationName = "Pasila";
            SearchLogic.SearchBetweenStations(from, to);
            //List<Train> trains = SearchLogic.SearchTrainsBetweenStations(from, to);

            //foreach (var train in trains)
            //{
            //    Console.WriteLine(train.trainNumber);
            //}
            SearchLogic.CalculateDistance(6, 5, 2, 2);

            SearchLogic.GetTrainRoute();

            Station testiasema = SearchLogic.stationDictionary["JOENSUU"];
            Console.WriteLine("testiasema:" + testiasema.stationName);
            Console.WriteLine("longitude " + testiasema.longitude);
            Console.WriteLine("lat " + testiasema.latitude);
            Console.WriteLine("distance = " + SearchLogic.GetTrainDistanceFromStation(testiasema)+"km");//junan etäisyys pasilan asemalta
        }
    }
}
