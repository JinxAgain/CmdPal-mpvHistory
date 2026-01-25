// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using mpvHistoryExtension.Utilities;

namespace mpvHistoryExtension;

public partial class mpvHistoryExtensionCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;
    private readonly ExtensionSettingsManager _settingsManager;
    private readonly mpvHistoryExtensionPage _page;

    public mpvHistoryExtensionCommandsProvider()
    {
        DisplayName = "mpvHistory";
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        
        // Initialize settings manager with Command Palette integration
        _settingsManager = new ExtensionSettingsManager();
        Settings = _settingsManager.Settings;
        
        _page = new mpvHistoryExtensionPage(_settingsManager);
        
        _commands = [
            new CommandItem(_page) { Title = DisplayName },
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }
}
