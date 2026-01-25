using System;

namespace mpvHistoryExtension.Models;

public class HistoryItem
{
    public DateTime Timestamp { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public double Time { get; set; }
    public double Length { get; set; }
}
