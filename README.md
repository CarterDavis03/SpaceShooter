# About

SpaceShooter is a complete rewrite of the application formally known as SpaceShooterAI. This version uses classes and was not thrown together as quickly as possible.

# Keybinds

Movement:

  * Left - Left
  * Right - Right

Combat:

  * Space - Shoot

Misc:

  * Q, Gamepad Back - Quit
  * Enter, Gamepad Start - Enter

# Required for Compilation

Font - https://fonts.google.com/specimen/Russo+One#standard-styles

Linux: dotnet-sdk-3.1 mono mgcb-editor openal

Windows: Dotnet mgcb-editor openal

!!! WILL NOT COMPILE WITHOUT OPENAL, PLEASE MAKE DIRECTX PROJECT AND COPY ALL BUT CSPROJ FILE IF YOU DON'T WISH TO USE OPENGL AND OPENAL !!!

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

