@echo off

IF "%1"=="" ( 
    ECHO Please, specify destination directory in arg
    EXIT /B 1
)

set config=Release

IF NOT "%2"=="" ( 
    set config=%2
)
ECHO %config%


IF NOT EXIST %1 (
   ECHO Destination %1 not exists or is not a directory
   MKDIR %1
   IF NOT ERRORLEVEL 0 (
      ECHO Can not to create dir %1
      EXIT /B 1
   )
)

set bindir="%~dp0\..\.bin\WebConsole_deploy\%config%"

RMDIR /S /Q %bindir%\Projects\
RMDIR /S /Q %bindir%\Temp\
ERASE /F /S /Q %bindir%\bin\*.pdb
ERASE /F /S /Q %bindir%\ConnectionStrings.config

XCOPY %bindir% %1  /E /C /I /F /H /R /Y /v
IF NOT ERRORLEVEL 0 (
    ECHO Error %ERRORLEVEL% while copying
    EXIT /B 1
    )








