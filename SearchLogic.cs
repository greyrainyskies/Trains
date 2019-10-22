﻿using RataDigiTraffic;
using RataDigiTraffic.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Trains
{
    // Author: Ville
    public enum TimetableRowType { Arrival, Departure };
    public static class SearchLogic
    {
        // Author: Ville
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

        // Author: Ville
        static TimetableRow[] SearchForTimetableRow(string shortCode, List<TimetableRow> rows, TimetableRowType rowType)
        {
            var query = from row in rows
                        where row.stationShortCode == shortCode && row.type == (rowType == TimetableRowType.Arrival ? "ARRIVAL" : "DEPARTURE")
                        select row;
            return query.ToArray();
        }
    }
}
