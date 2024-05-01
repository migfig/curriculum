using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class SummaryResponse {
    [JsonProperty("title")]
    public string Title {get; set;} = string.Empty;

    [JsonProperty("subTitle")]
    public string SubTitle {get; set;} = string.Empty; 

    [JsonProperty("columns")]
    public int Columns {get; set;}

    [JsonProperty("labels")]
    public string[] Labels {get; set;} = new string[]{};

    [JsonProperty("counts")]
    public double[] Counts {get; set;} = new double[]{};

    [JsonProperty("sum")]
    public double Sum {get; set;}

    [JsonProperty("count")]
    public int Count {get; set;}

    [JsonProperty("avg")]
    public double Avg {get; set;}

    [JsonProperty("min")]
    public double Min {get; set;}

    [JsonProperty("max")]
    public double Max {get; set;}

    [JsonProperty("chartType")]
    public eChartType ChartType {get; set;}
}

public enum eChartType
{
    Donut = 0,
    Line = 1,
    Pie = 2,
    Bar = 3,
}
