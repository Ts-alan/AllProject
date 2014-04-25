@echo off
::%1 $(TargetDir)$(ProjectName).pdb             PDB path
::%2 "Vba32 3.5 $(ConfigurationName)"           project name     
::%3 $(OutDir)\$(ProjectName)$(TargetExt)       binary file

set SYMBOL_BAT=C:\utils\sym_upload.bat
if not exist %SYMBOL_BAT% (
    echo No %SYMBOL_BAT% %1 %2 %3
    goto :eof
)

echo Call %SYMBOL_BAT% %1 %2 %3
call %SYMBOL_BAT% %1 %2 %3

exit %ERRORLEVEL%                                                       
echo.