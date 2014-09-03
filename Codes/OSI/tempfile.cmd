@echo off
setlocal
set home=%cd%
set direct=%1
if "%direct%" EQU "/?" (
	echo create tempfile in directory
	goto :eof
)

:start0
set name=%random%
if exist %direct%%name%.tmp (
	goto :start0
) else (
	if "%direct%" NEQ "" (
		cd %direct%
	)
	echo.>%name%.tmp 2>nul
	cd %home%
)








