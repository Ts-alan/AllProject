SET EXCL OFF
set century on
set date to german
if file('askrm.dbf') 
  if used('askrm')
    sele askrm
  else  
    use askrm
  endif
else 
  wait window('Не найден справочник "Календарь работы модуля"!') timeout 2
  quit
endif

CLEAR
DEFINE WINDOW output FROM 2,1 TO 7,35 TITLE 'Месяц';
       CLOSE FLOAT GROW SHADOW ZOOM
ACTIVATE WINDOW output
mon=month(date())
@ 1,1 say "Введите номер месяца - "
@ 1,25 get mon pict "99"
read  
deac wind output

insdate=ctod('01/'+str(mon,2)+'/2013')

do while  month(insdate)=mon

  if dow(insdate)<7 and dow(insdate)>1
    append blank 
    replace dm with insdate,;
            sm with "1",;
            kfvr with 8
  endif                      
  insdate=insdate+1

enddo                      
quit