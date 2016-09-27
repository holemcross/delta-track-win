using DeltaTrack.Helpers;
using DeltaTrack.Models;
using DeltaTrack.ViewModels;
using MetroLog;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using HttpClient = System.Net.Http.HttpClient;

namespace DeltaTrack.Services
{
    public class CtaService
    {
        private static string CTA_BASE_URL = "http://lapi.transitchicago.com/api";
        private static string CHICAGO_PORTAL_BASE_URL = "https://data.cityofchicago.org";
        private static string CTA_TRAIN_API_KEY_NAME = "CtaApiKey";

        private ILogger Logger;

        public CtaService()
        {
            Logger = LogManagerFactory.DefaultLogManager.GetLogger<CtaService>();
        }


        /// <summary>
        /// Gets Train Arrivals for a given MapId using CTA API
        /// </summary>
        /// <param name="mapId">Station MapId</param>
        /// <returns><see cref="CtaArrivalsViewModel"/></returns>
        public async Task<CtaArrivalsViewModel> GetArrivalsForMapId(int mapId)
        {
            Logger.Trace($"Get arrivals with {nameof(mapId)} : {mapId}");

            var arrivalsApiEndpoint = "/1.0/ttarrivals.aspx";
            var MAPID_PARAM = "mapid";
            //var STOPID_PARAM = "stpid";
            //var MAX_PARAM = "max";
            //var ROUTE_PARAM = "rt";
            var APIKEY_PARAM = "key";

            var ctaApiKey = KeyManager.GetKeyByName(CTA_TRAIN_API_KEY_NAME);

            var queryParams = new Dictionary<string, string>
            {
                {MAPID_PARAM, mapId.ToString()},
                {APIKEY_PARAM, ctaApiKey}
            };

            var endpoint = new Uri(CTA_BASE_URL + arrivalsApiEndpoint);

            var result = new CtaArrivalsViewModel();

            using (var client = new HttpClient())
            {
                try
                {
                    var getUri = UriHelpers.CreateWithQuery(endpoint, queryParams);
                    Logger.Trace($"Arrivals Get Uri String: {getUri}");

                    var response = await client.GetAsync(getUri);
                    var stream = await response.Content.ReadAsStreamAsync();

                    var document = XDocument.Load(stream);

                    if (document != null && document.Element("ctatt").HasElements)
                    {
                        var root = document.Element("ctatt");
                        result.ErrorCode = int.Parse(root.Element("errCd").Value);

                        if (result.ErrorCode != 0)
                        {
                            // Error has occurred
                            Logger.Error($"Cta Arrivals API Error with code: {result.ErrorCode}");
                            return result;
                        }

                        result.TimeStamp = GetDateTimeFromCtaTime(root.Element("tmst").Value);

                        result.TrainArrivals = root.Elements("eta").Select(e => new TrainArrival
                        {
                            ArrivalTime =       GetDateTimeFromCtaTime(e.Element("arrT").Value),
                            DestinationName =   e.Element("destNm").Value,
                            DestinationStopId = int.Parse(e.Element("destSt").Value),
                            IsApproaching =     Convert.ToBoolean(int.Parse(e.Element("isApp").Value)),
                            IsDelayed =         Convert.ToBoolean(int.Parse(e.Element("isDly").Value)),
                            IsNotLiveData =     Convert.ToBoolean(int.Parse(e.Element("isSch").Value)),
                            PredictionTime =    GetDateTimeFromCtaTime(e.Element("prdt").Value),
                            Route =             RouteHelper.GetRouteByAbbreviation(e.Element("rt").Value),
                            RunNumber =         int.Parse(e.Element("rn").Value),
                            TrainDirection =    int.Parse(e.Element("trDr").Value)
                        }).ToList();

                    }

                }
                catch (HttpRequestException ex)
                {
                    // Error Occurred
                    Logger.Error($"Cta Arrivals Request Exception with message: {ex.Message}", ex);
                    return result;
                }
                
            }

            return result;
        }

        public async Task<List<Station>> GetStations()
        {
            var stationsPortalEndpoint = "/resource/8pix-ypme.json";

            var endpoint = new Uri(CHICAGO_PORTAL_BASE_URL + stationsPortalEndpoint);

            Logger.Trace($"Chicago Portal API Get Stations Uri String: {endpoint}");
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(endpoint);
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    var stopJsonArray = JArray.Parse(jsonStr);
                    var stopsAndStations = stopJsonArray.Children<JObject>().Select(s => new
                    CtaStationStopViewModel {
                        MapId = s.GetValue("map_id").Value<int>(),
                        StationName = s.GetValue("station_name").Value<string>(),
                        StationDescriptiveName = s.GetValue("station_descriptive_name").Value<string>(),
                        Stop = new Stop
                        {
                            StopId = s.GetValue("stop_id").Value<int>(),
                            StopName = s.GetValue("stop_name").Value<string>(),
                            Location = new Point
                            {
                                X = s.GetValue("location").Value<JObject>().GetValue("latitude").Value<double>(),
                                Y = s.GetValue("location").Value<JObject>().GetValue("longitude").Value<double>()
                            },
                            Routes = GetStopRoutesFromJson(s),
                            IsHandicapAccesible = s.GetValue("ada").Value<bool>(),
                        }
                    });

                    return stopsAndStations.Aggregate(new List<Station>(),
                        AccumulateStation, 
                        list => list);
                }
                catch (HttpRequestException ex)
                {
                    // Something bad happened!
                    Logger.Error($"Chicago Portal Request Exception with message: {ex.Message}", ex);
                    return new List<Station>();
                }
            }

        }

        private static DateTime GetDateTimeFromCtaTime(string timeStr)
        {
            DateTime timeStamp;
            DateTime.TryParseExact(timeStr, "yyyyMMdd HH:mm:ss",
                CultureInfo.CurrentCulture, DateTimeStyles.None, out timeStamp);
            return timeStamp;
        }

        private List<CtaRoutes> GetStopRoutesFromJson(JObject stopJson)
        {
            var result = new List<CtaRoutes>();

            if (stopJson.GetValue("g")?.Value<bool>() ?? false)
            {
                result.Add(CtaRoutes.Green);
            }
            if (stopJson.GetValue("pnk")?.Value<bool>() ?? false)
            {
                result.Add(CtaRoutes.Pink);
            }
            if (stopJson.GetValue("o")?.Value<bool>() ?? false)
            {
                result.Add(CtaRoutes.Orange);
            }
            if (stopJson.GetValue("red")?.Value<bool>() ?? false)
            {
                result.Add(CtaRoutes.Red);
            }
            if (stopJson.GetValue("p")?.Value<bool>() ?? false)
            {
                result.Add(CtaRoutes.Purple);
            }
            if (stopJson.GetValue("blue")?.Value<bool>() ?? false)
            {
                result.Add(CtaRoutes.Blue);
            }
            if (stopJson.GetValue("y")?.Value<bool>() ?? false)
            {
                result.Add(CtaRoutes.Yellow);
            }
            if (stopJson.GetValue("brn")?.Value<bool>() ?? false)
            {
                result.Add(CtaRoutes.Brown);
            }

            return result;
        }

        /// <summary>
        /// Accumulates Stops into stations
        /// </summary>
        /// <param name="list"></param>
        /// <param name="stopViewModel"></param>
        /// <returns></returns>
        private List<Station> AccumulateStation(List<Station> list, CtaStationStopViewModel stopViewModel)
        {
            // Check if already in list
            var station = list.FirstOrDefault(s => s.MapId == stopViewModel.MapId);
            if (station != null)
            {
                station.Stops.Add(stopViewModel.Stop);
            }
            else
            {
                // Create new station
                var newStation = new Station
                {
                    MapId = stopViewModel.MapId,
                    StationName = stopViewModel.StationName,
                    DescriptiveName = stopViewModel.StationDescriptiveName,
                    Stops = new List<Stop>
                    {
                        stopViewModel.Stop
                    }
                };
                list.Add(newStation);
            }
            return list;
        }
    }
}
