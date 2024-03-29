﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RataDigiTraffic.Model
{
  public  class Station
    {

        public bool passengerTraffic;
        public string countryCode;
        //public string stationName;
        public string stationShortCode;
        public int stationUICCode;
        public decimal latitude;
        public decimal longitude;
        public string type;

        //below removes "asema" from the stationName
        private string _stationName;
        public string stationName
        {
            get { return _stationName; }
            set {
                    if (value.Contains(" asema"))
                    {
                        _stationName = value.Replace("asema", "").Trim();
                    }
                    else 
                    { 
                        _stationName = value; 
                    }
                }
        }

    }


//    passengerTraffic: boolean Onko liikennepaikalla kaupallista matkustajaliikennettä
//countryCode: string Liikennepaikan maatunnus
//stationName: string Liikennepaikan nimi
//stationShortCode: string Liikennepaikan lyhenne
//stationUICCode: 1-9999   Liikennepaikan maakohtainen UIC-koodi
//latitude: decimal Liikennepaikan latitude "WGS 84"-muodossa
//longitude: decimal Liikennepaikan longitudi "WGS 84"-muodossa
//type: string Liikennepaikan tyyppi.STATION = asema, STOPPING_POINT = seisake, TURNOUT_IN_THE_OPEN_LINE = linjavaihde

    //http://dev.solita.fi/2015/08/10/open-train-data.html
}
