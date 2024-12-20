﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace api;

public record StorageModel
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; init; }

    [JsonProperty(PropertyName = "description")]
    public string Description { get; init; }

    [JsonProperty(PropertyName = "placement")]
    public string Placement { get; init; }

    [JsonProperty(PropertyName = "items")]
    public List<ItemModel> Items { get; init; }
}
