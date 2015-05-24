dat1=ctod('01.01.2005')
dat2=DATE()
SELECT Mpartn.npls, Mpartn.kmk, Mpartn.kgt, Mpartn.kus, Mpartn.kpr,;
  Mpartn.kprb,Mpartn.datbr;
 FROM  mpartn;
 INTO cursor spr;
 WHERE Mpartn.datbr BETWEEN dat1 AND dat2;
union ;
SELECT AMpartn.npls, AMpartn.kmk, AMpartn.kgt, AMpartn.kus, AMpartn.kpr,;
  AMpartn.kprb,AMpartn.datbr;
 FROM  Ampartn;
 WHERE AMpartn.datbr BETWEEN dat1 AND dat2
 
 
 SELECT Mpartn.nprt, Mpartn.kmk, Mpartn.kgt, Mpartn.kus, Mpartn.kpr,;
  Mpartn.kprb,Mpartn.datbr;
 FROM  mpartn;
 INTO cursor spr1;
 WHERE Mpartn.datbr BETWEEN dat1 AND dat2

SELECT AMpartn.nprt, AMpartn.kmk, AMpartn.kgt, AMpartn.kus, AMpartn.kpr,;
  AMpartn.kprb,AMpartn.datbr;
 FROM  Ampartn;
 INTO cursor spr2;
 WHERE AMpartn.datbr BETWEEN dat1 AND dat2
 
 SELECT AMpartn.nprt, AMpartn.kmk, AMpartn.kgt, AMpartn.kus, AMpartn.kpr,;
  AMpartn.kprb,AMpartn.datbr;
 FROM AMPARTN;
 into cursor ggg;
 WHERE AMPARTN.DATBR BETWEEN ctod('01.01.2005') AND date();
union;
SELECT Mpartn.nprt, Mpartn.kmk, Mpartn.kgt, Mpartn.kus, Mpartn.kpr,;
  Mpartn.kprb,Mpartn.datbr;
 FROM MPARTN;
 WHERE MPARTN.DATBR BETWEEN ctod('01.01.2005') AND date()

SELECT * from spr1 INTO CURSOR fff;
union;
SELECT * from spr2 
