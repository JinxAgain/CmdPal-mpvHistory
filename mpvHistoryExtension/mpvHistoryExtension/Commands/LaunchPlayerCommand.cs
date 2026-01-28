using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using mpvHistoryExtension.Models;
using mpvHistoryExtension.Utilities;
using System;
using System.Diagnostics;

namespace mpvHistoryExtension.Commands;

public sealed partial class LaunchPlayerCommand : InvokableCommand
{
    private readonly HistoryItem _item;
    private readonly ExtensionSettingsManager _settingsManager;

    public LaunchPlayerCommand(HistoryItem item, ExtensionSettingsManager settingsManager)
    {
        _item = item;
        _settingsManager = settingsManager;
        Name = "Play";
    }

    public override ICommandResult Invoke(object? sender)
    {
        var player = _settingsManager.PlayerExecutable;
        
        // Just pass the file path, let mpv/scripts handle resume
        var processArgs = $"\"{_item.FilePath}\"";

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = player,
                Arguments = processArgs,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };

            var fileDir = System.IO.Path.GetDirectoryName(_item.FilePath);
            if (!string.IsNullOrEmpty(fileDir))
            {
                psi.WorkingDirectory = fileDir;
            }
            
            var process = Process.Start(psi);
            
            // Close the command palette after launching
            return CommandResult.Dismiss();
        }
        catch (Exception ex)
        {
            // Log or show error
            System.Diagnostics.Debug.WriteLine($"Failed to launch player: {ex.Message}");
            return CommandResult.Dismiss();
        }
    }
}
