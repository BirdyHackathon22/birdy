using Microsoft.AspNetCore.Components.Routing;
using Newtonsoft.Json;

namespace Birdy.API.Models
{
    public class BirdyWatch
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public AnimalType AnimalType => AnimalType.Bird;

        [JsonIgnore]
        public string AnimalCaption => AnimalType.ToString();

        [JsonProperty(PropertyName = "species")]
        public string Species { get; set; }

        [JsonProperty(PropertyName = "date_spotted")]
        public DateTime DateSpotted { get; set; }

        [JsonProperty(PropertyName = "location")]
        public Location Location { get; set; }

        [JsonProperty(PropertyName = "image_name")]
        public string ImageName { get; set; } = "no_image";

        [JsonProperty(PropertyName = "score")]
        public float Score { get; set; }

        [JsonProperty(PropertyName = "bounding_box")]
        public string BoundingBox { get; set; }

        [JsonProperty(PropertyName = "device")]
        public string Device { get; set; }

        [JsonProperty(PropertyName = "speciesvote")]
        public SpeciesVote SpeciesVote { get; set; } = new SpeciesVote();

        public BirdyWatch(string species, Location location)
        {
            Id = Guid.NewGuid().ToString();
            Species = species;
            Location = location;
        }
    }

    public class Location
    {
        [JsonProperty(PropertyName = "latitute")]
        public double Latitute { get; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; }

        public Location(double latitute, double longitude)
        {
            Latitute = latitute;
            Longitude = longitude;
        }

    }

    public enum AnimalType
    {
        Bird
    }

    public class SpeciesVote
    {
        [JsonProperty(PropertyName = "true_classification")]
        public int TrueClassification { get; set; }

        [JsonProperty(PropertyName = "false_classification")]
        public int FalseClassification { get; set; }

        public SpeciesVote()
        {
            TrueClassification = 0;
            FalseClassification = 0;
        }
    }
}

