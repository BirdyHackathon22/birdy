namespace Birdy.Web.Shared
{
    public class BirdyWatch
    {
        public string Id { get; set; } = string.Empty;

        public string AnimalCaption { get; set; } = string.Empty;

        public string Species { get; set; } = string.Empty;

        public DateTime DateSpotted { get; set; }

        public Location Location { get; set; } = new Location();

        public string ImageName { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public float Score { get; set; }

        public string BoundingBox { get; set; } = string.Empty;

        public string Device { get; set; } = string.Empty;
    }

    public class Location
    {
        public double Latitute { get; set; }

        public double Longitude { get; set; }
    }
}