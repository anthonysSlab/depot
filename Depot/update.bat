@echo off

echo -1 > UPDATED

xcopy /E /y .\tmp\ .\ >> .\update.log 2>&1
rm .\update.bat -v >> .\update.log 2>&1
rm .\update.sh -v >> .\update.log 2>&1

if %errorlevel% neq 0 exit /b %errorlevel%

echo 0 > UPDATED

start .\GodOfUwU.exe