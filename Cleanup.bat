REM Not working yet.

echo off

cd "C:\Stuff\Program data\VS\GitHub\Arca\"

start for /f "delims=," %i in ('dir /a:d /s /b bin') do rd /s /q "%i"

if %ERRORLEVEL% NEQ 0 (
	echo Failed to delete 'bin' folders.
	goto End
)


start for /f "delims=," %i in ('dir /a:d /s /b obj') do rd /s /q "%i"

if %ERRORLEVEL% NEQ 0 (
	echo Failed to delete 'obj' folders.
	goto End
)

del /q OutputPackages\*.*

if %ERRORLEVEL% NEQ 0 (
	echo Failed to delete the NuGet packages.
	goto End
)

:End

pause
