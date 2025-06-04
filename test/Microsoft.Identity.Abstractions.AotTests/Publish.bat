REM Needs to be published to make sure we don't have any AoT warnings

dotnet publish --runtime win-x64 -f net8.0
bin\Release\net8.0\win-x64\publish\Microsoft.Identity.Abstractions.AotTests.exe

dotnet publish --runtime win-x64 -f net9.0
bin\Release\net9.0\win-x64\publish\Microsoft.Identity.Abstractions.AotTests.exe
