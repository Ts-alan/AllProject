USE mnomer IN 0 SHARED
USE mpartt IN 0 SHARED
SELECT mpartt
SCAN FOR SUBSTR(nprt,5,1)==" "
  IF SEEK(mpartt.nprt, "mnomer", "NPRT")
    IF mnomer.przn#"1"
      REPLACE przn WITH "1" IN mnomer
    ENDIF
  ENDIF
ENDSCAN 

SELECT mnomer
SCAN FOR przn=="1" NOOPTIMIZE
  IF .NOT.SEEK(mnomer.nprt, "mpartt", "NPRT")
    REPLACE przn WITH "0" IN mnomer
  ENDIF
ENDSCAN

USE IN mnomer
USE IN mpartt