@echo off
setlocal enabledelayedexpansion

cd /d "%~dp0"

set "EXE="

for %%P in (
  "Builds\Windows\LoxQuest3D.exe"
  "Builds\Windows\LoxQuest3D\LoxQuest3D.exe"
  "Build\LoxQuest3D.exe"
  "LoxQuest3D.exe"
) do (
  if exist %%~P (
    set "EXE=%%~P"
    goto :found
  )
)

:notfound
echo Не найден билд игры (.exe).
echo Собери билд в Unity: File ^> Build Settings ^> Build
echo и положи его в Builds\Windows\LoxQuest3D.exe
exit /b 1

:found
echo Запуск: %EXE%
start "" "%EXE%"
exit /b 0

