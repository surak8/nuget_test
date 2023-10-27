@echo on
setlocal
set NAME2=make_nuget_spec
set NAME=nuget_test
set API_KEY=3cf63582-b815-4cd4-82ec-4b269fbc614b
set LOCATION=nuget.org
set LOCATION=http://colt-ubuntu.colt.com
set LOCATION=\\appdeploy\appdeploy\Colt Software\NUGET
set LOCATION=http://localhost:50648/nuget

set SRC=-source %LOCATION%
set VERSION=1.0.0.0

:ArgsStart
if {"%1x"}=={"x"} goto :ArgsDone

if {"%1"}=={"only"} set BuildOnly=1
if {"%1"}=={"verbose"} set VERBOSE=1
if {"%1"}=={"delete"} set DELETE=1
if {"%1"}=={"PACK"} set PACK=1
if {"%1"}=={"push"} set PUSH=1
shift
goto :ArgsStart
:ArgsDone

set SPEC_ARGS=spec 
set PACK_ARGS=pack
set DEL_ARGS=delete
set PUSH_ARGS=push
if {"%VERBOSE%"}=={"1"} set SPEC_ARGS=%SPEC_ARGS% -verbosity detailed
if {"%VERBOSE%"}=={"1"} set PACK_ARGS=%PACK_ARGS% -verbosity detailed
if {"%VERBOSE%"}=={"1"} set DEL_ARGS=%DEL_ARGS% -verbosity detailed
if {"%VERBOSE%"}=={"1"} set PUSH_ARGS=%PUSH_ARGS% -verbosity detailed
 
nuget %PUSH_ARGS% %NAME%.%VERSION%.nupkg %SRC% %API_KEY%
