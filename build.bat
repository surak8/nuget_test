@echo off
setlocal
REM set NAME2=make_nuget_spec
set NAME2=nuget_spec_generator
set NAME=nuget_test
set API_KEY=3cf63582-b815-4cd4-82ec-4b269fbc614b
set LOCATION=nuget.org
set LOCATION=http://colt-ubuntu.colt.com
set LOCATION=\\appdeploy\appdeploy\Colt Software\NUGET

set SRC=-source "%LOCATION%"
set VERSION=1.0.0.0

:ArgsStart
if {"%1x"}=={"x"} goto :ArgsDone

if {"%1"}=={"only"} set BuildOnly=1
if {"%1"}=={"help"} set ShowHelp=1
if {"%1"}=={"verbose"} set VERBOSE=1
if {"%1"}=={"delete"} set DELETE=1
if {"%1"}=={"PACK"} set PACK=1
if {"%1"}=={"push"} set PUSH=1
shift
goto :ArgsStart
:ArgsDone

if {"%ShowHelp%"}=={"1"} (
echo.
echo %~dpn0
exit /b 0
)

set SPEC_ARGS=spec 
set PACK_ARGS=pack
set DEL_ARGS=delete
set PUSH_ARGS=push
if {"%VERBOSE%"}=={"1"} set SPEC_ARGS=%SPEC_ARGS% -verbosity detailed
if {"%VERBOSE%"}=={"1"} set PACK_ARGS=%PACK_ARGS% -verbosity detailed
if {"%VERBOSE%"}=={"1"} set DEL_ARGS=%DEL_ARGS% -verbosity detailed
if {"%VERBOSE%"}=={"1"} set PUSH_ARGS=%PUSH_ARGS% -verbosity detailed

if exist "obj" rd /q/s obj
del /q/s *.nuspec *.nupkg %NAME%.dll %NAME%.dll.config *.pdb %NAME%.pdb %NAME%.xml %NAME2%.exe %NAME2%.xml %NAME2%.exe
if {"%BuildOnly%"}=={"1"} exit /b 0

echo.
echo build executable %NAME2%.exe
csc -nologo -out:%NAME2%.exe nuget_spec_generator.cs nuget_spec_attributes.cs Properties\AssemblyInfo.cs 


echo.
echo build executable DLL?
msbuild -nologo -v:m %NAME%.sln
echo.
echo emergency stop
exit /b 1

echo.
echo do NUGET
nuget %SPEC_ARGS% %NAME%.dll 

echo.
echo run %NAME2%.exe
call %NAME2%.exe

echo.
echo run %NAME2%.exe
nuget %PACK_ARGS% nuget_test.csproj 

if {"%DELETE%"}=={"1"} nuget %DEL_ARGS% %NAME% %VERSION% %API_KEY% %SRC%
if {"%PUSH%"}=={"1"} nuget %PUSH_ARGS% %NAME%.%VERSION%.nupkg %SRC% -apikey %API_KEY%
exit /b 0
REM copy /y %NAME2%.nuspec %NAME%.nuspec

REM nuget setApiKey %API_KEY% -Source %LOCATION%

REM nuget push -verbosity detailed %NAME%.1.0.0.0.nupkg -source nuget.org -apikey %API_KEY%
REM nuget push %NAME%.1.0.0.0.nupkg %SRC% -apikey %API_KEY%
REM nuget pack nuget_test.csproj 



rem 
