\ Rent.fs

REQUIRE ffl/act.fs

VARIABLE PROFIT 
ACT-CREATE PLAN
: INITIALIZE PLAN ACT-(FREE) 0 PROFIT ! ;
: PLAN@ ( time -- value ) PLAN ACT-GET 0= IF 0 THEN ;
: PLAN! ( value time -- ) PLAN ACT-INSERT ;
: UPDATE-PROFIT ( time -- ) PLAN@ PROFIT @ MAX PROFIT ! ;
: RENT ( time duration price -- ) -ROT + DUP PLAN@ ROT PROFIT @ + MAX SWAP PLAN! ;
: ACTION>KEY ( time duration price -- key ) 
    ?DUP 0= IF + 0 0 THEN
    -ROT SWAP 21 LSHIFT OR 17 LSHIFT OR ;

: KEY>ACTION ( key -- time duration price ) 
    DUP 17 RSHIFT DUP 21 RSHIFT 
    SWAP 1 21 LSHIFT 1- AND 
    ROT  1 17 LSHIFT 1- AND ;



