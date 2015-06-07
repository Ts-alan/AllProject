**********************************************************************
*             Программа для выверки некоторых баз данных             *
**********************************************************************
*                     15 ноября 2010 года, 14:02                     *
**********************************************************************
*                              corrdbf1                              *
**********************************************************************

SET BORDER TO DOUBLE
SET CLOCK OFF
SET CURSOR OFF
SET DATE GERMAN
SET DISPLAY TO VGA50
SET ESCAPE OFF
SET EXACT OFF
SET EXCLUSIVE OFF
SET SAFETY OFF
SET SHADOWS OFF
SET STATUS OFF
SET TALK OFF

dirdbffile = "O:\SHOP\SHOP43\DBF\"

USE dirdbffile+"Mpartt" IN 0
SET ORDER TO TAG Nprt IN Mpartt
USE dirdbffile+"Mcompl" IN 0
WAIT 'Происходит корректировка базы данных "'+ALLTRIM (ALIAS ())+'"';
     WINDOW;
     NOWAIT
SELECT Mcompl
GO TOP

SCAN

  IF .NOT. SEEK (LEFT (Mcompl.Nprt, 5), "Mpartt")
    DELETE
  ENDIF

ENDSCAN

WAIT "Корректировка баз данных закончена. Нажмите любую клавишу...";
     WINDOW;
     NOWAIT
= INKEY (0)
QUIT
