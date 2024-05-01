namespace Curriculum.EF.Models;

public record NameValue(
    object Name,
    object Value,
    Dictionary<string, object>? NameAttr = null,
    Dictionary<string, object>? ValueAttr = null
);

public record FileMap(
    string Url,
    long Size
);

public static class HelperExtensions
{
    public static SummaryResponse Sumarize<T>(this IEnumerable<T> items, Func<T, string> groupExpr, SummaryItemRequest flags, Func<T, double>? totalExpr = null)
    {
        var groups = (
            from item in items
            group item by groupExpr(item) into statusGroup
            orderby statusGroup.Key ascending
            select new { Label = statusGroup.Key, Count = totalExpr != null ? statusGroup.Sum(item => totalExpr(item)) : (double)statusGroup.Count() }
        );
        return new SummaryResponse {
            Title = flags.Title,
            SubTitle = flags.SubTitle,
            Columns = flags.Columns,
            Labels = groups.Select(g => flags.IncludeCountWithLabel ? $"{g.Label} ({g.Count})" : g.Label).ToArray(),
            Counts = groups.Select(g => g.Count).ToArray(),
            Sum = flags.HasSum ? groups.Select(g => g.Count).Sum() : 0.0,
            Count = flags.HasCount ? groups.Select(g => g.Count).Count() : 0,
            Avg = flags.HasAvg ? groups.Select(g => g.Count).Average() : 0.0,
            Min = flags.HasMin ? groups.Select(g => g.Count).Min() : 0.0,
            Max = flags.HasMax ? groups.Select(g => g.Count).Max() : 0.0,
            ChartType = flags.ChartType
        };
    }

    public static ICollection<NameValue> CustomGroupBy<T, K>(this IEnumerable<T> items, Func<T, K> groupExpr)
    {
        return (
            from item in items
            group item by groupExpr(item) into statusGroup
            orderby statusGroup.Key ascending
            select new NameValue(statusGroup.Key, statusGroup.Count())
        ).ToList();
    }
}
