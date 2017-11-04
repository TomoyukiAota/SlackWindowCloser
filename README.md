# SlackWindowCloser
Automatically closes Slack window on Windows startup.

This application is for [the issue that Slack window is opened after Windows startup](https://redd.it/4b9r2u).

I observed this issue right after installation of Slack (2.8.2 64-bit Direct Download) in Windows 10, but currently I do not anymore. I neither upgraded the version nor changed some configuration, but it disappeared now. This application is left here just in case it appears again.

## Prerequisite
 - Windows 10
 - Visual Studio 2017 (to build this application)

## How to install
 - [Git] for Git
 - [VS] for Visual Studio 2017
 - [Explorer] for Explorer
1. [Git] Clone this repository.
2. [VS] Open SlackWindowCloser.sln with Visual Studio 2017.
3. [VS] Build in Release Configuraiton.
4. [Explorer] Copy \SlackWindowCloser\bin\Release folder to somewhere you can keep.
5. [Explorer] Press Win+R, write `shell:startup`, and hit enter. The `Startup` folder will appear.
6. [Explorer] In `Startup` folder, create shortcut of `SlackWindowCloser.exe` located in the folder you copied in step 4.

## How to uninstall
1. Delete the folder created in step 4 in "How to intall" section.
1. Delete the shortcut created in step 6 in "How to intall" section.


