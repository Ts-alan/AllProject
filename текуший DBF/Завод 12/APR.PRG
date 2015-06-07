
USE MNOMER
J1= 82100
J2= 82400
KPR = "000"
DO WHILE J1<=J2
                   nprts = str(j1,5)

               nprtt = nprts  + " 00000"



APPEND BLANK

             REPLACE mnomer->nust WITH LEFT(nprtt,2);
                   mnomer->npru WITH SUBSTR(nprtt,3,3);
                   mnomer->przn WITH "0";
                   mnomer->kpr WITH "000";
                   mnomer->nprt WITH nprtt

                j1 = j1 + 1
ENDDO
