@echo off
chcp 65001
set y=%1
if "%1" EQU "/?" (
	echo Get help for you,  write help in different files
	goto :eof
)
for /f "tokens=1" %%i in ('help 
	^|findstr /B [A-Z] 
	^|findstr /V "SC"
	^|findstr /V "DISKPART"
	^|findstr /V "For"
') do (
	help %%i>%%i.txt 2>nul.
	echo %%i
)
pause