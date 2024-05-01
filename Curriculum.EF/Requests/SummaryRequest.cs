using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class SummaryRequest {
    [JsonProperty("items")]
    public SummaryItemRequest[] items {get; set;} = new SummaryItemRequest[]{};
}
