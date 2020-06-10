# MonogameCompareBehaviors
Let's compare the behavior of MonoGame for each environment.

# build
MonoGame 3.8.0 can compile programmable shaders (.fx files) on Linux and MacOS.

## macOS
1. install WINE and p7zip
2. install mgfxc
```
curl -O https://raw.githubusercontent.com/MonoGame/MonoGame/develop/Tools/MonoGame.Effect.Compiler/mgfxc_wine_setup.sh
chmod +x mgfxc_wine_setup.sh
./mgfxc_wine_setup.sh
```
3. add environment variables
```
 echo 'export PATH=$PATH:"/Applications/Wine Stable.app/Contents/Resources/wine/bin/"' >> ~/.zprofile 
 echo 'export MGFXC_WINE_PATH="$HOME/.winemonogame"' >> ~/.zprofile
```

The command line (dotnet tool run mgcb) works fine.

However, I cannot understand how to set the “MGFXC_WINE_PATH” environment variable in Visual Studio for Mac.
It works when you copy and paste the command in the error message to the terminal.
Please tell me if you found a solution.

