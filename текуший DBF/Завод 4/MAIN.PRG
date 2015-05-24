num=ADIR(lfiles,'*.dbf')
for ncount=1 to num
  do CPZERO with lfiles(ncount,1),866
endfor