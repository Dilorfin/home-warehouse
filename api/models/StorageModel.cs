using Newtonsoft.Json;
using System.Collections.Generic;

namespace api;

public record StorageModel
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; init; }
    [JsonProperty(PropertyName = "items")]
    public List<ItemModel> Items { get; init; }
}
