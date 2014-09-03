@echo off
setlocal enabledelayedexpansion
set y=%1
shift
set y=%y%%1
if "%y%" EQU "/?" (
echo Write time to sleep in seconds format
goto end
)
set x= %time%
echo %x%
for /f "tokens=1-4 delims=:," %%A in ("%time%") do (
set /A H=1%%A-100
set /A M=1%%B-100
set /A S=1%%C-100
set /A MS=1%%D-100
set /A ending=(!H!*(60+!M!^)*60+!S!+%y%^)*100+!MS!
rem echo !ending!
)
:start
for /f "tokens=1-4 delims=:," %%A in ("%time%") do (
set /A H=1%%A-100
set /A M=1%%B-100
set /A S=1%%C-100
set /A MS=1%%D-100
set /A tim=(!H!*(60+!M!^)*60+!S!^)*100+!MS!
if !tim! lss 360000 set /A tim = !tim! + 24*360000
rem echo !tim!
)
if !tim! lss !ending! goto start
set x= %time%
echo %x%
:end