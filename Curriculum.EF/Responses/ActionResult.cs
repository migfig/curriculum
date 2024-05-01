namespace Curriculum.EF.Models;

public class ActionResult<T> where T : class
{
    public bool Succeeded { get; private set; } = false;
    public T? Item { get; private set; }
    public string? Error { get; private set; } = string.Empty;

    public ActionResult(bool succeeded, T? item, string? error)
    {
        Succeeded = succeeded;
        Item = item;
        Error = error;
    }
}