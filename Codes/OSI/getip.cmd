@echo off
setlocal
if "%1" EQU "/?" (
echo get all IP
goto :eof
)
for /f "tokens=2 delims=:" %%A in ('ipconfig ^|findstr IPv4') do (
echo%%A
)



if "%direct%" EQU "" (goto start2) else (goto start)


:start
set name=%random%
for /f "delims=" %%i in ('dir %direct% /a /b') do (
	if %%i EQU %name% goto start
)
cd %direct%
echo.>%name%.tmp 2>nul
cd %home%
goto :eof


:start2
set name=%random%
for /f "delims=" %%i in ('dir /a /b') do (
	if %%i EQU %name% goto start2
)
echo.>%name%.tmp 2>nul