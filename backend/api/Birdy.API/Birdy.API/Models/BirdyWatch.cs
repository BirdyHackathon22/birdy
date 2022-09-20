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
        public int Score { get; set; }

        [JsonProperty(PropertyName = "bounding_box")]
        public string BoundingBox { get; set; }

        [JsonProperty(PropertyName = "device")]
        public string Device { get; set; }

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

    public static class BirdSpecies
    {
        public static readonly Dictionary<string, string> Images = new()
        {
            { "Mourning Dove", "https://www.bigyearbirding.com/wp-content/uploads/2018/08/Mourning-Dove.jpg" }, 
            { "Northern Cardinal", "https://fthmb.tqn.com/3ene7AxQ4oCRC6FDt-ePdC2IWXE=/1500x1000/filters:fill(auto,1)/northern-cardinal-male-58a6dae73df78c345b5f3610.jpg" }, 
            { "American Robin", "https://www.thespruce.com/thmb/21xBw_86Zkd3uQ9oscuyDhtZhII=/1500x1000/filters:fill(auto,1)/am-robin-594ec95a3df78cae81dca55a.jpg" }, 
            { "American Crow", "https://www.gannett-cdn.com/presto/2019/09/29/PPOH/58ee8a0a-6974-473a-a38e-96247ff8ff36-American_Crow.GaryHenry.jpg?crop=2047,1152,x0,y0&width=3200&height=1680&fit=bounds" }, 
            { "Blue Jay", "https://th.bing.com/th/id/R.09ecabc19a713acc8413a5261e2db13f?rik=IwCdxw1Uw%2fRphw&riu=http%3a%2f%2f4.bp.blogspot.com%2f-E0UWoPVuuIY%2fUcAY1BigDGI%2fAAAAAAAADRs%2fyR0cmFffGXI%2fs1600%2fBlue-Jay-Bird-Picture.jpg&ehk=OzkGl74Xuni%2bTA8UMZ%2bvCz7XYC2zWki1dJaWMOpht%2b8%3d&risl=&pid=ImgRaw&r=0" }, 
            { "Song Sparrow", "https://www.greatbirdpics.com/wp-content/uploads/2020/06/32332DA6-B531-46C1-8241-B089928ADAFB.jpeg" }, 
            { "Red-winged Blackbird", "https://th.bing.com/th/id/R.1b6f2ca31a071d7734814fb7758aadbd?rik=m7%2bxEb2VjNnZlw&riu=http%3a%2f%2f3.bp.blogspot.com%2f-lVl7oWV1pDM%2fU28ousp8JYI%2fAAAAAAAAAD4%2fOYUYEDunkUI%2fs1600%2fRed-Winged-Blackbird.-.jpg&ehk=O4DipdOX5qMMIFMa2DbvfsHeFxVFToclgoRc4gZmNiQ%3d&risl=&pid=ImgRaw&r=0" }, 
            { "European Starling", "https://images.squarespace-cdn.com/content/v1/5a6390338a02c77bf05da4ab/1567011692060-57KOIPTYT8J7MGWY2FRF/ke17ZwdGBToddI8pDm48kLkXF2pIyv_F2eUT9F60jBl7gQa3H78H3Y0txjaiv_0fDoOvxcdMmMKkDsyUqMSsMWxHk725yiiHCCLfrh8O1z4YTzHvnKhyp6Da-NYroOW3ZGjoBKy3azqku80C789l0iyqMbMesKd95J-X4EagrgU9L3Sa3U8cogeb0tjXbfawd0urKshkc5MgdBeJmALQKw/33721607850_f40c2ac436_o.jpg" }, 
            { "American Goldfinch", "https://1.bp.blogspot.com/-ve2Gff55bq4/YPwsHAycO8I/AAAAAAAAOcQ/rhOkJlXCEDUdAYr2OMGkdPeVRpBdlhwZACLcBGAsYHQ/s1800/American%2Bgoldfinch_1.jpg" },
            { "Canada Goose", "https://th.bing.com/th/id/R.9bc9499e31856dfb6e51a2b8aca426ce?rik=Hx0%2bnKNAAHK9OQ&riu=http%3a%2f%2f2.bp.blogspot.com%2f-ZM5B5urt8aQ%2fUUPhCj5fZtI%2fAAAAAAAACj8%2fjsTwQlKM6gc%2fs1600%2fCanada%2bGoose%2bBathing.jpg&ehk=8joWOd6%2f9iBZeqiCm7S4vi9cNkuf1c58neZM0xTbKVc%3d&risl=&pid=ImgRaw&r=0" }
        };

        public static readonly string[] CommonBirds = Images.Keys.ToArray();
    }
}