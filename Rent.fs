\ Rent.fs

REQUIRE ffl/act.fs

VARIABLE PROFIT 
ACT-CREATE PLAN

: INIT 
    PLAN ACT-INIT
    0 PROFIT ! ;

: PLAN@ ( t -- n )
    PLAN ACT-GET 0= IF 0 THEN ;

: PLAN! ( n t -- )
    PLAN ACT-INSERT ;

: UPDATE-PROFIT ( n -- )
    PROFIT @
    MAX PROFIT ! ;

: UPDATE-PLAN ( n t -- )
    DUP PLAN@ 
    ROT MAX
    SWAP PLAN! ;

: CASH ( t -- )
    PLAN@ UPDATE-PROFIT ;

: RENT ( t d p -- )
    ROT DUP CASH
    SWAP PROFIT @ +
    -ROT + UPDATE-PLAN ;
