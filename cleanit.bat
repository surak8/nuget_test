@echo on
setlocal
set NAME2=make_nuget_spec
set NAME=nuget_test
set API_KEY=3cf63582-b815-4cd4-82ec-4b269fbc614b
set LOCATION=nuget.org
set SRC=-source %LOCATION%


rd /q/s obj
del /q/s *.nuspec *.nupkg %NAME%.dll %NAME%.dll.config *.pdb %NAME%.pdb %NAME%.xml overview.htm %NAME2%.exe %NAME2%.xml %NAME2%.exe
exit /b 0
REM msbuild -nologo -v:m %NAME2%.sln
csc -nologo -out:%NAME2%.exe nuget_spec_generator.cs nuget_spec_attributes.cs Properties\AssemblyInfo.cs 
msbuild -nologo -v:m %NAME%.sln
nuget spec %NAME%.dll
call %NAME2%.exe
REM exit /b 0
REM copy /y %NAME2%.nuspec %NAME%.nuspec

REM nuget setApiKey %API_KEY% -Source %LOCATION%

REM nuget push -verbosity detailed %NAME%.1.0.0.0.nupkg -source nuget.org -apikey %API_KEY%
REM nuget push %NAME%.1.0.0.0.nupkg %SRC% -apikey %API_KEY%
nuget pack nuget_test.csproj 
nuget push -verbosity detailed %NAME%.1.0.0.0.nupkg %SRC% -apikey %API_KEY%
REM nuget pack nuget_test.csproj 

rem nuget delete  nuget_test 1.0.0.0 3cf63582-b815-4cd4-82ec-4b269fbc614b -source nuget.org -verbosity detailed 
