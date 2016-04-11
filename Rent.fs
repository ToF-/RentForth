\ Rent.fs

REQUIRE ffl/act.fs

VARIABLE PROFIT 
ACT-CREATE PLAN
: INITIALIZE PLAN ACT-(FREE) 0 PROFIT ! ;
: PLAN@ ( time -- value ) PLAN ACT-GET 0= IF 0 THEN ;
: PLAN! ( value time -- ) PLAN ACT-INSERT ;
: UPDATE-PROFIT ( time -- ) PLAN@ PROFIT @ MAX PROFIT ! ;
: PLAN-RENT ( price time -- ) DUP PLAN@ ROT PROFIT @ + MAX SWAP PLAN! ;


INITIALIZE
100 5  PLAN-RENT 
140 10 PLAN-RENT 
    5  UPDATE-PROFIT 
 80 14 PLAN-RENT
 70 15 PLAN-RENT
    10 UPDATE-PROFIT
    14 UPDATE-PROFIT
    15 UPDATE-PROFIT
PROFIT ?
BYE
