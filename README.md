# mpvHistory for Command Palette

A Command Palette extension that allows you to browse your **mpv** or **mpv.net** watch history and resume playback directly from where you left off.

## Features

*   📺 **Playback History**: Browse your recently played videos and launch them instantly. (Resume functionality depends on your mpv configuration/scripts)
*   🚀 **Support for mpv & mpv.net**: Compatible with both the CLI `mpv` and the modern `mpv.net` player.
*   ⚙️ **Integrated Settings**: Configure your player and history path directly within the Command Palette Settings panel.
*   🔇 **Silent Launch**: Launches the player instantly without opening a distracting terminal window.

## Prerequisites

1.  **Command Palette** installed.
2.  **mpv** or **mpv.net** installed and added to your system `PATH` environment variable.
3.  **Logging Enabled**: You need to configure mpv to save a log file so this extension can read it.
4.  **SimpleHistory Script**: This extension works best with the [SimpleHistory](https://github.com/Eisa01/mpv-scripts?tab=readme-ov-file#simplehistory) script to handle resume playback functionality.

### Enabling History Logging

Add the following line to your `mpv.conf` file:

```ini
log-file="C:\\path\\to\\your\\mpvHistory.log"
```

**Common configuration locations:**
*   **mpv**: `%APPDATA%\mpv\mpv.conf`
*   **mpv.net**: `%APPDATA%\mpv.net\mpv.conf`

> **Note**: Make sure to remember this path, as you will need to enter it in the extension settings.

## Configuration

1.  Open **Command Palette** and click the **Settings** (Gear) icon.
2.  Navigate to **Extensions** > **mpvHistory**.
3.  **Player Executable**: Select your preferred player (`mpv` or `mpv.net`).
4.  **History File Path**: Enter the full absolute path to your `mpvHistory.log` file.

## Usage

1.  Open Command Palette.
2.  Select or type **mpvHistory**.
3.  Browse your recently watched videos.
4.  Press **Enter** on any item to launch the player and resume playback.

## License

[MIT](LICENSE.txt)