using Newtonsoft.Json;

namespace api;

public record StorageModel
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; init; }
    [JsonProperty(PropertyName = "items")]
    public List<ItemModel> Items { get; init; }
}
