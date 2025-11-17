echo off

REM Change the path to match your environment
cd "D:\Work\Visual Studio\GitHub\Arca\"

echo Deleting the 'bin' folders...
for /f "delims=," %%i in ('dir /a:d /s /b bin') do rd /s /q "%%i"

if %ERRORLEVEL% NEQ 0 (
	echo Failed to delete the 'bin' folders.
	goto End
)


echo Deleting the 'obj' folders...
for /f "delims=," %%i in ('dir /a:d /s /b obj') do rd /s /q "%%i"

if %ERRORLEVEL% NEQ 0 (
	echo Failed to delete the 'obj' folders.
	goto End
)


echo Deleting the NuGet packages...
del /q OutputPackages\*.*

if %ERRORLEVEL% NEQ 0 (
	echo Failed to delete the NuGet packages.
	goto End
)


echo Finished.

:End

pause
