using Newtonsoft.Json;

namespace api;

public record ItemModel
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; init; }

    [JsonProperty(PropertyName = "count")]
    public int Count { get; init; }

    [JsonProperty(PropertyName = "title")]
    public string Title { get; init; }

    [JsonProperty(PropertyName = "comment")]
    public string Comment { get; init; }
}
