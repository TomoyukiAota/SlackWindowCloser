# SlackWindowCloser
Automatically closes Slack window on Windows startup. This application is for [the issue that Slack window is left open after Windows startup](https://redd.it/4b9r2u).

This application launches at Windows startup and searches for the process which renders Slack Main Window. When the process is detected, this application closes the window and exits.

Also, if this application runs for more than 100 seconds without detecting the process, this application exits. This is an safety feature not to run this application forever. The duration which this application runs can be configured, so longer duration may be preferable for slow PCs. (See Options section.)

I observed this issue right after installation of Slack (2.8.2 64-bit Direct Download) in Windows 10, but currently I do not anymore. I neither upgraded the version nor changed some configurations, but it disappeared now. This application is left here just in case it appears again.

## Prerequisite
 - Windows 10
 - Visual Studio 2017 (to build this application)

## How to install
 - [Git] for Git
 - [VS] for Visual Studio 2017
 - [Explorer] for Explorer
1. [Git] Clone this repository.
2. [VS] Open SlackWindowCloser.sln
3. [VS] Build in Release Configuraiton.
4. [Explorer] Copy Release folder in \SlackWindowCloser\bin\ to somewhere you can keep. (Then, you should rename the newly created folder to `SlackWindowCloser` for clarity of its contents.)
5. [Explorer] Press Win+R, write `shell:startup`, and hit enter. The `Startup` folder will appear.
6. [Explorer] In `Startup` folder, create a shortcut of `SlackWindowCloser.exe` located in the folder you copied in step 4.

## How to uninstall
1. Delete the folder created in step 4 in "How to install" section.
2. Delete the shortcut created in step 6 in "How to install" section.

## Options
 - `--duration`: The duration this application runs can be configured. For example, if you want to run this application for 200 seconds, add `--duration 200` in Target in Properties of the shortcut.
 - `--log-folder`: A log file can be saved in the specified folder when this application runs. For example, if you want to save the log in `C:\Temp`, add `--log-folder C:\Temp` in Target in Properties of the shortcut.