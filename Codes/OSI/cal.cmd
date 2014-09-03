@echo off
setlocal enabledelayedexpansion
set x=%1
if "%1" == "" (set /p x=write expression & goto :end)
:start
shift
set x=%x%%1
if "%1" NEQ "" goto :start
:end
if "%x%" NEQ "/?" if "%x%" NEQ "" set /a ans = %x% 2>nul
if "%x%" NEQ "/?" if "%x%" NEQ "" if %errorlevel% EQU 0 (echo %ans%) else (echo error)
if "%x%" EQU "/?" echo Write Your task, for example: 2 + 2.
if "%x%" EQU "" echo Write Your task.
