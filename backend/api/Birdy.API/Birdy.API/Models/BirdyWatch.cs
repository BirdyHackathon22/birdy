using Microsoft.AspNetCore.Components.Routing;
using Newtonsoft.Json;

namespace Birdy.API.Models
{
    public class BirdyWatch
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "type")]
        public AnimalType AnimalType => AnimalType.Bird;

        [JsonIgnore]
        public string AnimalCaption => AnimalType.ToString();

        [JsonProperty(PropertyName = "species")]
        public string Species { get; set; }

        [JsonProperty(PropertyName = "last_watch_date")]
        public DateTime LastWatchDate { get; set; }

        [JsonProperty(PropertyName = "location")]
        public Location Location { get; set; }

        public BirdyWatch(string species, Location location)
        {
            Species = species;
            Location = location;
        }
    }

    public class Location
    {
        [JsonProperty(PropertyName = "latitute")]
        public long Latitute { get; }

        [JsonProperty(PropertyName = "longitude")]
        public long Longitude { get; }

        public Location(long latitute, long longitude)
        {
            Latitute = latitute;
            Longitude = longitude;
        }

    }

    public enum AnimalType
    {
        Bird
    }
}