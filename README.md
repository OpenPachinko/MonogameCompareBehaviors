# MonogameCompareBehaviors
Let's compare the behavior of MonoGame for each environment.

# build
MonoGame 3.8.0 can compile programmable shaders (.fx files) on Linux and MacOS!Wow!Great!

## macOS
1. install WINE and p7zip
```
brew update
brew install p7zip
brew cask install xquartz
brew cask install wine-stable
```

2. install mgfxc
```
curl -O https://raw.githubusercontent.com/MonoGame/MonoGame/develop/Tools/MonoGame.Effect.Compiler/mgfxc_wine_setup.sh
chmod +x mgfxc_wine_setup.sh
./mgfxc_wine_setup.sh
```
3. add environment variables

```
 echo 'export MGFXC_WINE_PATH="$HOME/.winemonogame"' >> ~/.zprofile
```
- none: Can be omitted in the latest version of mgfxc_wine_setup.sh.
- note: mgfxc is installed in the "$HOME/.winemonogame" directory.

## Remarks
The command line (dotnet tool run mgcb) works fine.

However, I cannot understand how to set the “MGFXC_WINE_PATH” environment variable in Visual Studio for Mac.
It works when you copy and paste the command in the error message to the terminal.
Please tell me if you found a solution.

