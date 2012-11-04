@ECHO OFF

MSBuild CallWall.Windows.build
IF %ERRORLEVEL% NEQ 0 GOTO :ERROR

NUnit-console .\bin\Test\CallWall.Core.UnitTests.dll


EXIT /B 0


:ERROR
ECHO "Build failed."
EXIT /B 1 