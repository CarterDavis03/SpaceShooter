# About

SpaceShooterAI is a modified build of the simple space shooter game, SpaceShooter, built with MonoGame/XNA that includes simple AI for the opponent ship.

# Keybinds

Movement:

  * A, Left, DPad_Left - Left
  * D, Right, DPad_Right - Right

Combat:

  * Space, Gamepad A - Shoot

Misc:

  * Q, Gamepad Back - Quit
  * F11 - Toggle Fullscreen
  * Enter, Gamepad Start - Enter

# Required for Compilation

Font - https://fonts.google.com/specimen/Russo+One#standard-styles

Linux: dotnet-sdk-3.1 mono mgcb-editor

# To Compile

Open Content.mgcb in mgcb-editor and build it. Then run the commands.

Linux:

``` sh
dotnet publish -c Release -r linux-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained
```

Windows:

``` sh
dotnet publish -c Release -r win-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained
```

