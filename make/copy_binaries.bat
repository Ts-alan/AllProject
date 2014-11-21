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

set bindir=%~dp0\..\.bin
set bindirCC=%bindir%\WebConsole_deploy\%config%

IF NOT EXIST %1SERVICE (
   ECHO Destination %1SERVICE not exists or is not a directory
   MKDIR %1SERVICE
   IF NOT ERRORLEVEL 0 (
      ECHO Can not to create dir %1SERVICE
      EXIT /B 1
   )
)


ECHO Service files copying...

FOR %%i IN (Vba32PMS  Vba32NS  Vba32SS packet_parser) DO (
    ERASE /F /S /Q "%bindir%\Services\%%i\%config%\*.pdb"
    ERASE /F /S /Q "%bindir%\Services\%%i\%config%\*.xml"
    XCOPY "%bindir%\Services\%%i\%config%" %1SERVICE  /E /C /I /F /H /R /Y /v
    IF NOT ERRORLEVEL 0 (
        ECHO Error %ERRORLEVEL% while copying
        EXIT /B 1
        )
)


IF NOT EXIST %1WEBCONSOLE (
   ECHO Destination %1WEBCONSOLE not exists or is not a directory
   MKDIR %1WEBCONSOLE
   IF NOT ERRORLEVEL 0 (
      ECHO Can not to create dir %1WEBCONSOLE
      EXIT /B 1
   )
)

ECHO WebConsole: unnecessary files deleting...

RMDIR /S /Q "%bindirCC%\Temp\"
ERASE /F /S /Q "%bindirCC%\bin\*.pdb"
ERASE /F /S /Q "%bindirCC%\ConnectionStrings.config"

ECHO AjaxControlToolkit localization files deleting...

CD %bindirCC%\bin\

FOR /D %%d in (*.*) DO (
 if "%%d" neq "ru" (
  if "%%d" neq "ru-RU" (
   rmdir /S /Q "%%d"
  )
 )
)


ECHO WebConsole files copying...

XCOPY "%bindirCC%"  %1\WEBCONSOLE  /E /C /I /F /H /R /Y /v
IF NOT ERRORLEVEL 0 (
    ECHO Error %ERRORLEVEL% while copying
    EXIT /B 1
    )



IF NOT EXIST %1Sql (
   ECHO Destination %1Sql not exists or is not a directory
   MKDIR %1Sql
   IF NOT ERRORLEVEL 0 (
      ECHO Can not to create dir %1Sql
      EXIT /B 1
   )
)


ECHO SQL files copying...
COPY "%bindir%\Sql\%config%\VbaControlCenterDB.dbproj.sql"  %1\Sql\VbaControlCenterDB.sql /Y /V
COPY "%bindir%\Sql\%config%\Vba32ControlCenterUpdate.exe"  %1\Sql\Vba32ControlCenterUpdate.exe /Y /V