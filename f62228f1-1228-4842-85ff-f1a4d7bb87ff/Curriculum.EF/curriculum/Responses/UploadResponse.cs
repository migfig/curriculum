using System;

namespace Curriculum.EF.Models;

public class UploadResponse
{
    public Guid Id { get; set; }
    public string? FileName { get; set; }
    public long FileSize { get; set; }
    public Uri? FileUrl { get; set; }
}