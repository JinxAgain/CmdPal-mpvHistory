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
        FilePath = GetSettingsFilePath();

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
            "Full path to your mpvHistory.log file",
            "");

        Settings.Add(_playerExecutable);
        Settings.Add(_historyFilePath);
    }

    private static string GetSettingsFilePath()
    {
        var directory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "mpvHistoryExtension");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, "settings.json");
    }
}
