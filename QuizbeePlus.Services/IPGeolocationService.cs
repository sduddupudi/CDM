using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using RestClient.Net;
using System.Threading.Tasks;
using QuizbeePlus.Entities.IPGeolocation;
using System.Net.Http;
using System;
using Urls;

namespace QuizbeePlus.Services
{
    public class IPGeolocationService
    {
        public static IPGeolocationService _Instance;      
        public string iPGeolocationRequest;
        private string Key = string.Empty;
        private string LocationUrl = string.Empty;
        public static IPGeolocationService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new IPGeolocationService();
                }
                return (_Instance);
            }
        }
        private IPGeolocationService()
        { 
            Key= ConfigurationManager.AppSettings["IPGeolocation.apiKey"];
            LocationUrl= ConfigurationManager.AppSettings["IPGeolocation.url"];
        }

        public Location GetLocationsDetailsAsync(string QueryString )
        {
            RestClient service = new RestClient(LocationUrl+ "?apiKey=" + Key+ (QueryString==string.Empty?"":"&"+ QueryString), HttpVerb.GET, ""){ ContentType = "application/x-www-form-urlencoded",Accept = @"application/json" };
            var response = service.MakeRequest();
            Entities.IPGeolocation.Location location = JsonConvert.DeserializeObject<Entities.IPGeolocation.Location>(response);
            return location;
        }

      
    }
}
