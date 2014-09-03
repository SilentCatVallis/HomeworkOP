@echo off
setlocal enabledelayedexpansion
if "%1" equ "\?" (
	echo Write you expression, this program calculate it
	goto :eof
)
if "%1" equ "" (
	set /p x=write expression 
	goto :end
)
set x=%1
:start
shift
set x=%x%%1
if "%1" NEQ "" goto :start
:end
set /a ans=%x% 2>nul
if %errorlevel% EQU 0 (echo %ans%) else (echo error)

if "%x%" EQU "" (
	echo You don't write task.
)