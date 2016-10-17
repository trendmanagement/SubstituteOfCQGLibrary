@echo off

rem This script rebuilds all the solutions and runs all the tests.
rem It's worth to launch it in two cases at least:
rem 1. After you created a new local clone of the GitHub repository. Call it to make sure that your work environment is ready.
rem 2. Before you make a commit to the repository. Call it to make sure that you didn't introduce any regression.

rem Adjust the next three variables if you need
set configuration=Debug
set platform="Any CPU"
set verbosity=normal

rem Set path to "nuget.exe"
set path=%path%;%~dp0\ThirdParty

rem Set path to "msbuild.exe" and "mstest.exe"
call "%VS140COMNTOOLS%..\..\VC\vcvarsall.bat"

rem Rebuild all solutions
for %%s in (FakeCQG DataCollectionForRealtime TimedBars) do (
    cd %%s
    
    rem Restore all NuGet packages for this solution
    for /R %%p in (packages.config) do (
        if exist %%p (
            pushd %%~dpp
            nuget restore -SolutionDirectory %%~dps
            popd
        )
    )
    
    rem Rebuild this solution
    msbuild /t:Rebuild /p:Configuration=%configuration%;Platform=%platform% /v:%verbosity%
    
    cd ..
    
    if ErrorLevel 1 goto exit
)

rem Run all tests
mstest /testcontainer:FakeCQG\UnitTestFakeCQG\bin\%configuration%\UnitTestFakeCQG.dll
mstest /testcontainer:DataCollectionForRealtime\UnitTestRealCQG\bin\%configuration%\UnitTestRealCQG.dll

:exit
echo.
pause