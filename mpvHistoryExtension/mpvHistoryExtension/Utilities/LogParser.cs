using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using mpvHistoryExtension.Models;

namespace mpvHistoryExtension.Utilities;

public static class LogParser
{
    private static readonly Regex LogPattern = new Regex(
        @"^\[(.*?)\]\s+(.*?)\s+\|\s+length=(.*?)\s+\|\s+time=(.*?)$",
        RegexOptions.Compiled);

    private static readonly string[] PathSeparator = [" | "];

    public static List<HistoryItem> ParseLog(string filePath)
    {
        var items = new List<HistoryItem>();
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            // Try to find in current directory or Documents as a fallback guess?
            // User requested explicit config, so return empty is fine until configured.
            // But for testing I might want to fallback.
            return items; 
        }

        try
        {
             using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
             using var sr = new StreamReader(fs);
             
             string? line;
             while ((line = sr.ReadLine()) != null)
             {
                var match = LogPattern.Match(line);
                if (match.Success)
                {
                    var timestampStr = match.Groups[1].Value;
                    var path = match.Groups[2].Value;
                    var lengthStr = match.Groups[3].Value;
                    var timeStr = match.Groups[4].Value;

                    if (!double.TryParse(timeStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double time)) time = 0;
                    if (!double.TryParse(lengthStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double length)) length = 0;

                    DateTime timestamp = DateTime.MinValue;
                    var parts = timestampStr.Split(' ');
                    if (parts.Length >= 2)
                    {
                        // Timestamp format: "Wednesday/August 27/08/2025 14:29:49"
                        // parts: [0]="Wednesday/August", [1]="27/08/2025", [2]="14:29:49"
                        // Or if day name missing: "27/08/2025 14:29:49"
                        
                        // We take the last two parts
                        var datePart = parts[parts.Length - 2];
                        var timePart = parts[parts.Length - 1];
                        
                        DateTime.TryParseExact($"{datePart} {timePart}", "dd/MM/yyyy HH:mm:ss", 
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out timestamp);
                    }

                    // Clean path (remove quotes if any? Regex captures quotes if logic is loose, but our regex is greedy on path?
                    // Pattern: ^\[(.*?)\]\s+(.*?)\s+\|\s+length=(.*?)\s+\|\s+time=(.*?)$
                    // If path contains " | ", it's likely "Title | URL". 
                    // Since "|" is invalid in Windows filenames, we can safely split.
                    if (path.Contains(" | "))
                    {
                       path = path.Split(PathSeparator, StringSplitOptions.None).Last();
                    }

                    items.Add(new HistoryItem
                    {
                        Timestamp = timestamp,
                        FilePath = path.Trim(), // Trim regex capture
                        Time = time,
                        Length = length
                    });
                }
             }
        }
        catch (Exception)
        {
            // Ignore
        }

        items.Reverse(); // Newest first
        return items;
    }
}
