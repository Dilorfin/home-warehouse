using Newtonsoft.Json;

namespace api;

public record ItemModel
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; init; }
    [JsonProperty(PropertyName = "data")]
    public string Data { get; init; }
}

