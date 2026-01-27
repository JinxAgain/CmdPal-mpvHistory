using System;
using System.IO;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace mpvHistoryExtension.Utilities;

/// <summary>
/// Settings manager that integrates with Command Palette's settings panel
/// </summary>
public class ExtensionSettingsManager : JsonSettingsManager
{
    private static readonly string _namespace = "mpvhistory";

    private static string Namespaced(string propertyName) => $"{_namespace}.{propertyName}";

    private readonly ChoiceSetSetting _playerExecutable;
    private readonly TextSetting _historyFilePath;

    public string PlayerExecutable => _playerExecutable.Value ?? "mpv";
    public string HistoryFilePath => _historyFilePath.Value ?? "";

    public ExtensionSettingsManager()
    {
        try 
        {
            // Use Roaming AppData which is more reliable for settings persistence
            FilePath = GetSettingsFilePath();
        }
        catch (Exception ex)
        {
            // Fallback or log if needed, though we can't easily log here
            System.Diagnostics.Debug.WriteLine($"Error setting file path: {ex.Message}");
        }

        _playerExecutable = new ChoiceSetSetting(
            Namespaced("player"),
            "Player Executable",
            "Select the player to use for video playback",
            [
                new ChoiceSetSetting.Choice("mpv", "mpv"),
                new ChoiceSetSetting.Choice("mpv.net (mpvnet)", "mpvnet")
            ]);

        _historyFilePath = new TextSetting(
            Namespaced("historyPath"),
            "History File Path",
            "Full path to your mpvHistory.log file (e.g. C:\\...\\mpvHistory.log)",
            "");

        Settings.Add(_playerExecutable);
        Settings.Add(_historyFilePath);
        
        // Load settings from file upon initialization
        LoadSettings();

        // Save settings whenever they change
        Settings.SettingsChanged += (s, a) =>
        {
            SaveSettings();
        };
    }

    private static string GetSettingsFilePath()
    {
        var directory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "mpvHistoryExtension");
            
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        return Path.Combine(directory, "settings.json");
    }
}
