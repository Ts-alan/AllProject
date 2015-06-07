*USE Asident
*SET ORDER TO TAG Asbd IN Asident
*= SEEK ("MPARTT", "Asident")
pathdbfnew = "O:\SHOP\SHOP43\DBF\"
*pathdbfnew = ALLTRIM (Asident.Asspath)
CLOSE DATABASES
DO cpzero WITH "Amcompl.dbf", 866
DO cpzero WITH "Amkrist.dbf", 866
DO cpzero WITH "Amkrist.dbf", 866
DO cpzero WITH "Amparam.dbf", 866
DO cpzero WITH "Ampartn.dbf", 866
DO cpzero WITH "Ampartt.dbf", 866
DO cpzero WITH "Amplast.dbf", 866
DO cpzero WITH "Amprxop.dbf", 866
DO cpzero WITH "Amregn.dbf", 866
DO cpzero WITH "Amrest.dbf", 866

