echo off

echo Restoring NuGet packages

dotnet restore "Automated.Arca.sln" --force

if %ERRORLEVEL% NEQ 0 (
	echo The NuGet packages were not properly restored.
	goto End
)


echo Building solution

"C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe" "Automated.Arca.sln" /property:Configuration=Release /property:Deterministic=True /t:rebuild /consoleloggerparameters:ErrorsOnly;WarningsOnly

if %ERRORLEVEL% NEQ 0 (
	echo The packages were not properly created. Make sure to first install "Build Tools for Visual Studio 2019", and select the option ".Net Core build tools".
	goto End
)


echo The packages were put in the OutputPackages folder.

:End

pause
