using Newtonsoft.Json;
using RataDigiTraffic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RataDigiTraffic
{
    public class APIUtil

    {
public List<Station> Stations()
        {
            string json = "";
            using (var client = new HttpClient())
            {           
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"https://rata.digitraffic.fi/api/v1/metadata/stations").Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                json = responseString;
            }
            List<Station> res;
            res = JsonConvert.DeserializeObject<List<Station>>(json);
            return res;
        }

        public List<Train> TrainsBetween(string mistä, string minne)
        {
            string json = "";
            string url = $"https://rata.digitraffic.fi/api/v1/schedules?departure_station={mistä}&arrival_station={minne}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(url).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                json = responseString;
            }
            List<Train> res;
            res = JsonConvert.DeserializeObject<List<Train>>(json);
            return res;
        }

        public List<TrackingMessage> StationTrains(string paikka )
        {
            string json = "";
            string url = $"https://rata.digitraffic.fi/api/v1/train-tracking?station={paikka}&departure_date={DateTime.Today.ToString("yyyy-MM-dd")}";

            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(url).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                json = responseString;
            }
            List<TrackingMessage> res;
            res = JsonConvert.DeserializeObject<List<TrackingMessage>>(json);
            return res;
        }
    }





}
