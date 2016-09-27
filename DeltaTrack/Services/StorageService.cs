using DeltaTrack.Helpers;
using DeltaTrack.Models;
using MetroLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Storage;

namespace DeltaTrack.Services
{
    public class StorageService
    {
        private static string STATIONS_FILE_NAME = "Stations.xml";

        private StorageFolder _storegeRoot;

        private ILogger Logger;

        public StorageService()
        {
            _storegeRoot = ApplicationData.Current.LocalFolder;
            Logger = Logger = LogManagerFactory.DefaultLogManager.GetLogger<StorageService>();
        }
        public async Task StoreStationsIntoXml(List<Station> stations)
        {
            if (!stations.Any())
            {
                Logger.Info("No stations were included to be serialized!");
                return;
            }

            var document = new XDocument();
            try
            {
                document.Add( new XElement("Stations", stations.Select(s => new XElement(
                    "Station",
                    new XAttribute("MapId", s.MapId),
                    new XAttribute("StationName", s.StationName),
                    new XAttribute("DescriptiveName", s.DescriptiveName),

                    // Stops
                    new XElement("Stops", s.Stops.Select(st => new XElement("Stop",
                        new XAttribute("StopId", st.StopId),
                        new XAttribute("StopName", st.StopName),
                        new XAttribute("IsHandicapAccessible", st.IsHandicapAccesible),

                        // Routes
                        new XElement("Routes",
                            s.Routes.Select(r => new XElement("Route", r.ToString()))
                            ),

                        // Location
                        new XElement("Location",
                            new XAttribute("Latitude", st.Location.X),
                            new XAttribute("Longitude", st.Location.Y)
                            )
                        )))

                    ))));
            }
            catch (Exception ex)
            {
                Logger.Error($"Creation failed with error: {ex.Message}");
            }
            


            //Check if file already exists
            var files = await _storegeRoot.GetFilesAsync();
            
            // Get Stations File
            var storageFile = files.FirstOrDefault(f => f.Name == STATIONS_FILE_NAME) ??
                              await _storegeRoot.CreateFileAsync(STATIONS_FILE_NAME);

            using (var fileWriter = await storageFile.OpenStreamForWriteAsync())
            {
                document.Save(fileWriter);
            }
            
        }

        public async Task<List<Station>> RetrieveStations()
        {
            var files = await _storegeRoot.GetFilesAsync();
            
            // Get Stations File
            var storageFile = files.FirstOrDefault(f => f.Name == STATIONS_FILE_NAME);

            if (storageFile == null)
            {
                Logger.Info("Storage File was not found.");
                return new List<Station>();
            }

            var document = XDocument.Load(storageFile.Path);

            var stations = document.Element("Stations").Elements("Station").Select(s => new Station
            {
                MapId = XmlHelper.GetIntFromAttribute(s, "MapId"),
                StationName = s.Attribute("StationName").Value,
                DescriptiveName = s.Attribute("DescriptiveName").Value,
                Stops = s.Element("Stops").Elements("Stop").Select(st => new Stop
                {
                    StopId = XmlHelper.GetIntFromAttribute(st, "StopId"),
                    StopName = st.Attribute("StopName").Value,
                    IsHandicapAccesible = XmlHelper.GetBoolFromAttribute(st, "IsHandicapAccessible"),
                    Location = new Point(XmlHelper.GetDoubleFromAttribute(st.Element("Location"), "Latitude"),
                        XmlHelper.GetDoubleFromAttribute(st.Element("Location"), "Longitude")),
                    Routes =
                        st.Element("Routes").Elements("Route").Select(r => RouteHelper.GetRouteByName(r.Value)).ToList()
                }).ToList()
            });

            return stations.ToList();
        }
    }
}
