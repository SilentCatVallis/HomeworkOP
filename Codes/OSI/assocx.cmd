@echo off
if "%1" equ "/?" (
	echo get reserved function for ext
	goto :eof
)
set location=
for /f "tokens=4 delims= " %%i in ('reg query HKLM\SOFTWARE\Classes\.%1 /ve ^2^>nul ^| findstr "REG_SZ"') do (
    set location=%%i
)
if "%location%" equ "" (
	echo file not exist
	goto :eof
)
for /f "tokens=6 delims=\" %%i in ('reg query HKLM\SOFTWARE\Classes\%location%\shell /s ^| findstr "command Command"') do (
	for /f "tokens=4,*" %%a in ('reg query HKLM\SOFTWARE\Classes\%location%\shell\%%i\command /ve') do (
		echo %%i: %%a %%b
    )
)