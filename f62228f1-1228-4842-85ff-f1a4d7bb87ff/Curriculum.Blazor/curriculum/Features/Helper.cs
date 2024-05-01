using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mud = MudBlazor;
using MudBlazor.Utilities;
using Curriculum.EF.Models;
using MudBlazor;

namespace Curriculum.Blazor;

public record PropertyConfig(
    string Name,
    string Value
);

public record ThemeConfig(
    string Name,
    PropertyConfig[] Properties
);

public static class Helper
{
    public const string AUTH_TOKEN_KEY = "Curriculum-authToken";
    public const string THEME_NAME_KEY = "Curriculum-themeName";
    public const string MUD_THEME_TYPE = "MudTheme";
    public const string THEME_NAME_TYPE = "ThemeName";

    public const char PROPERTY_LINES_SEPARATOR_CHAR = '\n';
    public const char KEY_PROPERTIES_SEPARATOR_CHAR = '^';
    public const char KEY_PROPERTIES_JSON_SEPARATOR_CHAR = '.';
    public const char KEY_VALUES_SEPARATOR_CHAR = '|';

    public static string Plural(this string name, bool endWithPlural = true)
    {
        if (string.IsNullOrEmpty(name)) return name;

        var endValue = endWithPlural && !name.EndsWith("s") && !name.EndsWith("ss") ? "s" : string.Empty;
        var value = name.EndsWith("y")
            ? Regex.Replace(name, "y$", "ie")
            : (
                name.EndsWith("us")
                    ? Regex.Replace(name, "us$", "uses")
                    : (
                        name.EndsWith("h")
                        ? Regex.Replace(name, "h$", "he")
                        : name.EndsWith("ss")
                            ? Regex.Replace(name, "ss$", "sses")
                            : name
                    )
            );
        return $"{value}{endValue}";
    }

    public static bool EqualsId<T>(this T item, Guid id)
    {
        try
        {
            return Guid.Parse(item.GetType().GetProperty("Id").GetValue(item).ToString()).Equals(id);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool EqualsName<T>(this T item, string name, string propName = "Name")
    {
        try
        {
            return item.GetType().GetProperty(propName).GetValue(item).ToString().Equals(name);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static string NameValue<T>(this T item, string propName = "Name")
    {
        try
        {
            return item.GetType().GetProperty(propName).GetValue(item).ToString();
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public static async Task<ActionResult<T>> PostAction<T, K>(this HttpClient client, string url, K request) where T : class
    {
        var jsonOptions = new JsonSerializerOptions { 
            PropertyNameCaseInsensitive = true, 
            Converters = {
                new JsonStringEnumConverter(),
            }
        };
        var jsonRequest = typeof(K) == typeof(string) ? request as string : JsonSerializer.Serialize(request, jsonOptions);
        var bodyContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        var postResponse = await client.PostAsync(url, bodyContent);
        var jsonResponse = await postResponse.Content.ReadAsStringAsync();
        if (!postResponse.IsSuccessStatusCode)
        {
            var error = string.Empty;
            if (JsonDocument.Parse(jsonResponse)
                .RootElement
                .TryGetProperty("message", out JsonElement valueMessage))
            {
                error = valueMessage.GetString();
            }
            else if (JsonDocument.Parse(jsonResponse)
                .RootElement
                .TryGetProperty("error", out JsonElement valueError))
            {
                error = valueError.GetString();
            }

            return new ActionResult<T>(false, null, error);
        }
        return new ActionResult<T>(true, JsonSerializer.Deserialize<T>(jsonResponse, jsonOptions), string.Empty);
    }

    public static async Task<bool> PostAction<K>(this HttpClient client, string url, K request)
    {
        var jsonOptions = new JsonSerializerOptions { 
            PropertyNameCaseInsensitive = true, 
            Converters = {
                new JsonStringEnumConverter(),
            }
        };
        var jsonRequest = typeof(K) == typeof(string) ? request as string : JsonSerializer.Serialize(request, jsonOptions);
        var bodyContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        var postResponse = await client.PostAsync(url, bodyContent);
        var jsonResponse = await postResponse.Content.ReadAsStringAsync();
        if (!postResponse.IsSuccessStatusCode)
        {
            var error = string.Empty;
            if (JsonDocument.Parse(jsonResponse)
                .RootElement
                .TryGetProperty("message", out JsonElement valueMessage))
            {
                error = valueMessage.GetString();
            }
            else if (JsonDocument.Parse(jsonResponse)
                .RootElement
                .TryGetProperty("error", out JsonElement valueError))
            {
                error = valueError.GetString();
            }

            return false;
        }
        return bool.Parse(jsonResponse.ToLower());
    }

    public static IOrderedEnumerable<IGrouping<K, T>> GroupedItems<T, K>(this IEnumerable<T> items, Func<T, K> groupExpr)
    {
        var groups = (
            from item in items
            group item by groupExpr(item) into statusGroup
            orderby statusGroup.Key ascending
            select statusGroup
        );
        return groups;
    }

    public static Mud.MudTheme ChangesToMudTheme(this Dictionary<string, string> changes, Mud.MudTheme activeTheme)
    {
        var type = activeTheme.GetType();
        foreach (var key in changes.Keys)
        {
            var propNames = key.Split(KEY_PROPERTIES_SEPARATOR_CHAR);
            var isTwoProps = propNames.Length == 2;
            var parentName = propNames.First();
            var childName = isTwoProps ? propNames.Last() : propNames.ElementAt(1);
            var grandChildName = isTwoProps ? string.Empty : propNames.Last();

            var parentProp = type?
                .GetProperty(parentName)?
                .GetValue(activeTheme);


            if (isTwoProps)
            {
                var childProp = parentProp?.GetType()
                    .GetProperty(childName);

                if (childProp?.PropertyType.Name == "MudColor")
                {
                    childProp.SetValue(parentProp, new MudColor(changes[key]));
                }
                else
                {
                    childProp?.SetValue(parentProp, changes[key]);
                }
            }
            else
            {
                var childProp = parentProp?.GetType()?
                    .GetProperty(childName)?
                    .GetValue(parentProp);

                var grandChildProp = childProp?.GetType()
                    .GetProperty(grandChildName);

                grandChildProp?.SetValue(
                    childProp,
                    grandChildName == "FontFamily"
                        ? changes[key].Split(',')
                        : changes[key]
                );
            }
        }

        return activeTheme;
    }

    public static Dictionary<string, string> ThemeConfigToChanges(this ThemeConfig theme)
    {
        return new Dictionary<string, string>(
            theme.Properties.Select(p =>
                new KeyValuePair<string, string>(
                    p.Name.Replace(KEY_PROPERTIES_JSON_SEPARATOR_CHAR, KEY_PROPERTIES_SEPARATOR_CHAR),
                    p.Value
                )
            )
        );
    }

    public static Mud.MudTheme ThemeConfigToMudTheme(this ThemeConfig theme, Mud.MudTheme activeTheme)
    {
        return ThemeConfigToChanges(theme)
            .ChangesToMudTheme(activeTheme);
    }

    public static string ToProgressValue(this string value, bool isLabel = false)
    {
        var parts = value.Split('/');
        var step = int.Parse(parts.First());
        var count = int.Parse(parts.Last());
        var progress = (step * 100) / count;
        return isLabel ? $"{progress}%" : $"{progress}";
    }

    public static string Capitalize(this string value)
    {
        if (string.IsNullOrEmpty(value)) return value;

        return value.Length > 1
            ? $"{value[0].ToString().ToUpper()}{value.Substring(1)}"
            : value.ToUpper();
    }

    public static string CapitalizeSpacecify(this string value, char placeholder = ' ')
    {
        if (string.IsNullOrEmpty(value)) return value;
        value = value.Capitalize();
        var newValue = string.Empty;

        for (int i = 1; i < value.Length; i++)
        {
            newValue += Char.IsUpper(value[i]) ? $"{placeholder}{value[i]}" : value[i].ToString();
        }

        return value.Length > 1
            ? $"{value[0].ToString()}{newValue}"
            : value.ToUpper();
    }

    public static eSortDirection ToSortDirection(this Mud.SortDirection sortDirection) {
        return Enum.Parse<eSortDirection>(sortDirection.ToString());
    }

    public static Color ToColor(this double value) {
        return value switch {
            >= 75.0 => Color.Success,
            >= 50.0 => Color.Info,
            _ => Color.Warning
        };
    }
}
