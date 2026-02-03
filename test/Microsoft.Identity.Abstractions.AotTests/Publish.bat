REM Needs to be published to make sure we don't have any AoT warnings

dotnet publish --runtime win-x64 -f net10.0
bin\Release\net10.0\win-x64\publish\Microsoft.Identity.Abstractions.AotTests.exe
