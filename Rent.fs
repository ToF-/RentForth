\ Rent.fs
REQUIRE ffl/hct.fs

VARIABLE PROFIT 
CREATE PLAN 20 ALLOT

: INITIALIZE PLAN 20 ERASE 0 PROFIT ! ;

: PLAN# ( time -- addr )  PLAN + ; 

: UPDATE-PROFIT ( time -- ) PLAN + C@ PROFIT @ MAX PROFIT ! ;

: PLAN-RENT ( price time -- ) PLAN + DUP C@ ROT PROFIT @ + MAX SWAP C! ;



