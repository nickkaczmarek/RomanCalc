# RomanCalc

Built using `dotnet new`.

To publish, use `dotnet publish` and you can choose to built to an executable, or just do `dotnet RomanCalc.dll` wherever that file ends up getting built.

To build an exe for macOS: `dotnet publish --runtime osx.10.11-x64`.

To test run `dotnet test`.

RomanCalc can accept a newline delimited file or you can pass arguments directly.

`RomanCalc test.txt` or `RomanCalc 123` or `RomanCalc MCMXCII` should all work.

If you don't pass anything, it will ask for a path or argument and will then parse and return.
