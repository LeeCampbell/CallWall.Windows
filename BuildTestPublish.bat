@ECHO OFF

MSBuild CallWall.Windows.build /p:BuildVersion=0.0.0.2
IF %ERRORLEVEL% NEQ 0 GOTO :ERROR

ECHO "NUnit not part of tool set"
REM NUnit-console .\bin\Test\CallWall.UnitTests.dll .\bin\Test\CallWall.Core.UnitTests.dll

ECHO "Creating application manifest"
REM This works
cd .\bin\release
Mage -New Application -ToFile CallWall.manifest -FromDirectory . -Version 0.0.0.1  -IconFile CallWall.ico -Name "CallWall (Local)"

REM These fail
REM		Mage -New Application -ToFile CallWall.manifest -Algorithm sha256RSA -FromDirectory bin/release	-IconFile CallWall.ico -TrustLevel "Internet" -Version 0.0.0.1
REM		Mage -New Application -ToFile CallWall.manifest -FromDirectory bin/release -IconFile CallWall.ico -TrustLevel "Internet" -Version 0.0.0.1
REM			Mage -New Application -ToFile CallWall.manifest -FromDirectory bin/release -Version 0.0.0.1 -TrustLevel "Internet"

EXIT /B 0


:ERROR
ECHO "Build failed."
EXIT /B 1 