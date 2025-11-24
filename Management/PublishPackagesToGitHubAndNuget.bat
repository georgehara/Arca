echo off

set PackageVersion=%1
set GitHubApiKey=%2
set NugetApiKey=%3

REM set GitHubSource="github"
set GitHubSource=https://nuget.pkg.github.com/georgehara/index.json
set NugetSource=https://api.nuget.org/v3/index.json


echo Publishing NuGet packages to GitHub


REM dotnet nuget add source --username georgehara --password %GitHubApiKey% --store-password-in-clear-text --name github "%SourceUrl%"


dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Abstractions.Core.%PackageVersion%.nupkg" --source %GitHubSource% --api-key %GitHubApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Abstractions.DependencyInjection.%PackageVersion%.nupkg" --source %GitHubSource% --api-key %GitHubApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Abstractions.Specialized.%PackageVersion%.nupkg" --source %GitHubSource% --api-key %GitHubApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Attributes.DependencyInjection.%PackageVersion%.nupkg" --source %GitHubSource% --api-key %GitHubApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Attributes.Specialized.%PackageVersion%.nupkg" --source %GitHubSource% --api-key %GitHubApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Extensions.DependencyInjection.%PackageVersion%.nupkg" --source %GitHubSource% --api-key %GitHubApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Extensions.Specialized.%PackageVersion%.nupkg" --source %GitHubSource% --api-key %GitHubApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Implementations.ForMicrosoft.%PackageVersion%.nupkg" --source %GitHubSource% --api-key %GitHubApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Libraries.%PackageVersion%.nupkg" --source %GitHubSource% --api-key %GitHubApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Manager.%PackageVersion%.nupkg" --source %GitHubSource% --api-key %GitHubApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Single.%PackageVersion%.nupkg" --source %GitHubSource% --api-key %GitHubApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

echo The NuGet packages were successfully published to GitHub.


echo Publishing NuGet packages to NuGet

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Abstractions.Core.%PackageVersion%.nupkg" --source %NugetSource% --api-key %NugetApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Abstractions.DependencyInjection.%PackageVersion%.nupkg" --source %NugetSource% --api-key %NugetApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Abstractions.Specialized.%PackageVersion%.nupkg" --source %NugetSource% --api-key %NugetApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Attributes.DependencyInjection.%PackageVersion%.nupkg" --source %NugetSource% --api-key %NugetApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Attributes.Specialized.%PackageVersion%.nupkg" --source %NugetSource% --api-key %NugetApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Extensions.DependencyInjection.%PackageVersion%.nupkg" --source %NugetSource% --api-key %NugetApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Extensions.Specialized.%PackageVersion%.nupkg" --source %NugetSource% --api-key %NugetApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Implementations.ForMicrosoft.%PackageVersion%.nupkg" --source %NugetSource% --api-key %NugetApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Libraries.%PackageVersion%.nupkg" --source %NugetSource% --api-key %NugetApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Manager.%PackageVersion%.nupkg" --source %NugetSource% --api-key %NugetApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)

dotnet nuget push "../Arca/OutputPackages/Automated.Arca.Single.%PackageVersion%.nupkg" --source %NugetSource% --api-key %NugetApiKey% --skip-duplicate
if %ERRORLEVEL% NEQ 0 (
	echo The NuGet package was not published.
	goto End
)


echo The NuGet packages were successfully published to NuGet.


:End

pause
