@echo off
if "%1" equ "/?" (
	echo Get path for command
	goto :eof
)
set flag=0
set command=%1
set location=%cd%
set path1=%path%;%location%; 
	:start
	if "%path1%" neq " " (
		for /f "tokens=1,2 delims=;" %%a in ("%path1%") do (
					call :check "%%a\%command%" %pathext%
					if "!flag!" equ "1" ( goto :eof )
			set path1=!path1:%%a;=!
		)
		goto :start
	) else (
		echo Not found
		goto :eof
	)

:check
	
:Repeat
	set extension=%2
	if exist "%~1%extension%" (
		set a="%~1%extension%"
		set a=!a:"=!
		echo !a!
		rem echo "%~1%extension%"
		set flag=1
		goto :eof
	)
	if "%extension%" equ "" (
		exit /b
	)
	shift /2
	goto :Repeat