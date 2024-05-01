using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class InsertIntoJoinedTableRequest
{
    [JsonProperty("tableName")]
    public string TableName { get; set; } = "Default";

    [JsonProperty("fieldNames")]
    public string[] FieldNames { get; set; } = new List<string>().ToArray();

    [JsonProperty("values")]
    public object[] Values { get; set; } = new List<object>().ToArray();
}