public class ApiConfiguration
{
    public ApiConfiguration(string baseUrl)
    {
        BaseUrl = baseUrl;
    }

    public string BaseUrl { get; set; } = string.Empty;
}
