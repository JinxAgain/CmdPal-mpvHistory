using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using mpvHistoryExtension.Commands;
using mpvHistoryExtension.Utilities;
using System;
using System.Collections.Generic;

namespace mpvHistoryExtension;

internal sealed partial class mpvHistoryExtensionPage : ListPage
{
    private readonly ExtensionSettingsManager _settingsManager;

    public mpvHistoryExtensionPage(ExtensionSettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = "mpvHistory";
        Name = "History";
    }

    public override IListItem[] GetItems()
    {
        var items = new List<IListItem>();

        var path = _settingsManager.HistoryFilePath;
        if (string.IsNullOrWhiteSpace(path))
        {
            items.Add(new ListItem(new NoOpCommand()) 
            { 
                Title = "History path not set.",
                Subtitle = "Go to Command Palette Settings > Extensions > mpvHistory to configure."
            });
            return items.ToArray();
        }

        var logs = LogParser.ParseLog(path);
        if (logs.Count == 0)
        {
             items.Add(new ListItem(new NoOpCommand()) 
             { 
                 Title = "No history found.",
                 Subtitle = $"Checked: {path}"
             });
        }
        else
        {
            foreach (var log in logs)
            {
                var fileName = log.FilePath;
                try 
                {
                    // Only get filename if it looks like a path
                    if (fileName.Contains('\\') || fileName.Contains('/'))
                    {
                        fileName = System.IO.Path.GetFileName(fileName);
                    }
                }
                catch {}

                if (string.IsNullOrEmpty(fileName)) fileName = "Unknown";

                var timeStr = TimeSpan.FromSeconds(log.Time).ToString(@"hh\:mm\:ss", System.Globalization.CultureInfo.InvariantCulture);

                items.Add(new ListItem(new LaunchPlayerCommand(log, _settingsManager))
                {
                    Title = fileName,
                    Subtitle = $"{log.Timestamp:g} | Resume at {timeStr}"
                });
            }
        }

        return items.ToArray();
    }
}
