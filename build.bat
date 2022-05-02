dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true;AssemblyName=AziProxyWin64
dotnet publish -r win-x86 -c Release /p:PublishSingleFile=true;AssemblyName=AziProxyWin32
dotnet publish -r win-arm -c Release /p:PublishSingleFile=true;AssemblyName=AziProxyWinArm
dotnet publish -r osx-x64 -c Release /p:PublishSingleFile=true;AssemblyName=AziProxyOSX64
dotnet publish -r linux-x64 -c Release /p:PublishSingleFile=true;AssemblyName=AziProxyLinux64
dotnet publish -r linux-arm -c Release /p:PublishSingleFile=true;AssemblyName=AziProxyLinuxArm