@ECHO OFF
SET VERSION=0.0.0.5

MSBuild CallWall.Windows.build /p:BuildVersion=%VERSION%
IF %ERRORLEVEL% NEQ 0 GOTO :ERROR


ECHO "NUnit not part of tool set"
REM NUnit-console .\bin\Test\CallWall.UnitTests.dll .\bin\Test\CallWall.Core.UnitTests.dll


ECHO "Creating application manifest"

pushd .\bin\release

	REM This works	
	Mage -New Application -ToFile CallWall.manifest -FromDirectory . -Version %VERSION%  -IconFile CallWall.ico -Name "CallWall (Local)"
	
	REM These fail
	REM		Mage -New Application -ToFile CallWall.manifest -Algorithm sha256RSA -FromDirectory bin/release	-IconFile CallWall.ico -TrustLevel "Internet" -Version 0.0.0.1
	REM		Mage -New Application -ToFile CallWall.manifest -FromDirectory bin/release -IconFile CallWall.ico -TrustLevel "Internet" -Version 0.0.0.1
	REM			Mage -New Application -ToFile CallWall.manifest -FromDirectory bin/release -Version 0.0.0.1 -TrustLevel "Internet"
	
	REM	If running from a website, you can rename all files to .deploy to help work around mimetype extension and firewalls blocking exes.
	REM Rename all files to have a .deploy extension??
	REM		FOR /r %%d in (*.*) DO RENAME "%%d" *.*.deploy
	REM		RENAME CallWall.manifest.deploy CallWall.manifest
popd

REM To deploy: 
REM		copy the Published application (from above) to the target dir (server://??/[version]/)
REM		Run mage for deployment pointing to the published files

ROBOCOPY .\bin\release d:\clickonce\CallWall.Windows\v%VERSION%

REM	Mage -New Deployment 
REM		-AppCodeBase file://d:/clickonce/CallWall.Windows/v%VERSION%/CallWall.manifest 
REM		-AppManifest ??? 
REM		-Name "CallWall (Local)"
REM		-Version %VERSION%					--WOuld actually need to be a higher number than the last deploy.

Mage -New Deployment -AppManifest "D:\clickonce\CallWall.Windows\v%VERSION%\CallWall.manifest" -Name "CallWall (Local)" -Version %VERSION% -ToFile D:\clickonce\CallWall.Windows\CallWall.application

EXIT /B 0


:ERROR
ECHO "Build failed."
EXIT /B 1 