using Microsoft.AspNetCore.Components.Routing;

namespace Birdy.API.Models
{
    public class Animal
    {
        public AnimalType AnimalType => AnimalType.Bird;

        public string AnimalCaption => AnimalType.ToString();

        public string Species { get; set; }

        public DateTime LastBirdyWatch { get; set; }

        public Location Location { get; set; }

        public Animal(string species, Location location)
        {
            Species = species;
            Location = location;
        }
    }

    public class Location
    {
        public long Latitute { get; }

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