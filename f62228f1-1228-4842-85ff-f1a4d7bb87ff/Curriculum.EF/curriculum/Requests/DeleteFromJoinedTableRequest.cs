using Newtonsoft.Json;

namespace Curriculum.EF.Models;

public class DeleteFromJoinedTableRequest
{
    [JsonProperty("tableName")]
    public string TableName { get; set; } = "Default";

    [JsonProperty("fieldNames")]
    public KeyValuePair<string, object>[] FieldValues { get; set; } = new List<KeyValuePair<string, object>>().ToArray();
}