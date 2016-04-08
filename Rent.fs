\ Rent.fs
REQUIRE ffl/hct.fs

VARIABLE PROFIT 
10000 CONSTANT MAXORDER#
MAXORDER# HCT-CREATE PLAN

: INITIALIZE
    MAXORDER# PLAN HCT-INIT
    0 PROFIT ! ;

: INT>STR ( n -- addr # )
    S>D <# #S #> ;

: PLAN# ( time -- addr # plan )
   INT>STR PLAN ; 

: PLAN@ ( time -- n  )
    PLAN# HCT-GET 0= IF 0 THEN PROFIT @ MAX ;
 
: PLAN! ( n time -- )
    DUP PLAN@ ROT MAX SWAP PLAN# HCT-INSERT ;
 
: CASH ( time -- )
    PLAN@ PROFIT ! ;
      
: RENT ( end price -- )
    PROFIT @ + SWAP PLAN! ;
    
    

