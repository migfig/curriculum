using System;

namespace Curriculum.EF.Models;

public class FileResponse
{
    public string? FileName { get; set; }
    public long FileSize { get; set; }
    public Uri? FileUrl { get; set; }
}