echo off

echo Restoring NuGet packages for Automated.Arca

dotnet restore "../Automated.Arca.slnx" --force

if %ERRORLEVEL% NEQ 0 (
	echo The NuGet packages were not properly restored.
	goto End
)

echo Building solution Automated.Arca

"C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" "../Automated.Arca.slnx" /property:Configuration=Release /property:Deterministic=True /t:rebuild /consoleloggerparameters:ErrorsOnly;WarningsOnly

if %ERRORLEVEL% NEQ 0 (
	echo The packages were not properly created. Make sure to first install "Build Tools for Visual Studio 2026", and select the option ".Net build tools". See https://visualstudio.microsoft.com/downloads/
	goto End
)


echo The packages were put in the OutputPackages folder.

:End

pause
