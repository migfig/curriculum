using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class SummaryItemRequest {
    [JsonProperty("title")]
    public string Title {get; set;} = string.Empty;

    [JsonProperty("subTitle")]
    public string SubTitle {get; set;} = string.Empty; 

    [JsonProperty("columns")]
    public int Columns {get; set;}

    [JsonProperty("includeCountWithLabel")]
    public bool IncludeCountWithLabel {get; set;}

    [JsonProperty("hasSum")]
    public bool HasSum {get; set;}

    [JsonProperty("hasCount")]
    public bool HasCount {get; set;}

    [JsonProperty("hasAvg")]
    public bool HasAvg {get; set;}

    [JsonProperty("hasMin")]
    public bool HasMin {get; set;}

    [JsonProperty("hasMax")]
    public bool HasMax {get; set;}

    [JsonProperty("chartType")]
    public eChartType ChartType {get; set;}
}
