@echo off
:: Set some Vars to use
set buildFolder=..\Build
set akiDataFolder=..\Build\Aki_Data
set projReleaseFolder=.\bin\Release\net6.0\win-x64
set launcherExeFolder=..\Aki.Launcher\bin\Release\net6.0\win-x64\publish
set launcherAssetFolder=..\Aki.Launcher\Aki_Data
set licenseFile=..\..\LICENSE.md

echo --------------- Cleaning Output Build Folder ---------------

:: Delete build folder and contents to make sure its clean
if exist %buildFolder% rmdir /s /q %buildFolder%

echo --------------- Done Cleaning Output Build Folder ---------------
echo --------------- Creating Output Build Folders ---------------

:: Create build folder if it doesn't exist
if not exist %buildFolder% mkdir %buildFolder%
if not exist %akiDataFolder% mkdir %akiDataFolder%

echo --------------- Done Creating Output Build Folders ---------------

echo --------------- Moving DLLs to %buildFolder% ---------------

:: Move DLLs/exe/json project's bin\Release folder to the build folder
xcopy "%launcherExeFolder%\Aki.Launcher.exe" %buildFolder%
xcopy "%launcherAssetFolder%" "%buildFolder%\Aki_Data" /s /e

:: If any new Dll's need to be copied, add here

echo --------------- Done Moving DLLs to %buildFolder% ---------------
echo --------------- Writing License File ---------------

:: write the contents of the license file to a txt
type %licenseFile% > "%buildFolder%\LICENSE-Launcher.txt"

echo --------------- Done Writing License File ---------------