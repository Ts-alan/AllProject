
SET EXCL OFF
USE MNOMER
J1= 5000
J2= 5299
KPR = "000"
DO WHILE J1<=J2
                   nprts = str(j1,4)

               nprtt = "0"+nprts  + " 00000"



APPEND BLANK

             REPLACE mnomer->nust WITH LEFT(nprtt,2);
                   mnomer->npru WITH SUBSTR(nprtt,3,3);
                   mnomer->przn WITH "0";
                   mnomer->nprt WITH nprtt

                j1 = j1 + 1
ENDDO
