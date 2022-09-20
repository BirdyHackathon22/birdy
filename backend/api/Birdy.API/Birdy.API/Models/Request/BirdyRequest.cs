using Newtonsoft.Json;

namespace Birdy.API.Models.Request
{
    public class BirdyRequest
    {
        /*
         * class Bird:
                label : str
                score : int
                path : str
                box : Any
                location : str
                time : str
                device : str
         */
        public string Label { get; set; } = string.Empty;
        public int Score { get; set; }
        public string Path { get; set; } = string.Empty;
        public string Box { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string Device { get; set; } = string.Empty;
    }
}
