SET EXCL OFF
USE MNOMER
J1= 5000
J2= 3299
KS= 0
DO WHILE J1<=J2
                ks = 4 - (LEN(LTRIM(STR(j1,4))))
                IF ks = 1
                   nprts = STUFF((str(j1,4)),1,1,"0")
                   ELSE
                         IF ks = 2
                            nprts = STUFF((str(j1,4)),1,2,"00")

                            ELSE
                               IF ks = 3
                                  nprts = STUFF((str(j1,4)),1,3,"000")
                                      ELSE
                                         nprts = STR(j1,4)
                               ENDIF

                         ENDIF
                 ENDIF

               nprtt = nprts  + PRZZ+"00000"



APPEND BLANK

             REPLACE mnomer->nust WITH LEFT(nprtt,2);
                   mnomer->npru WITH SUBSTR(nprtt,3,3);
                   mnomer->przn WITH "0";
                   mnomer->nprt WITH nprtt
                
                j1 = j1 + 1
ENDDO
