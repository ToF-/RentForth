\ Rent.fs

REQUIRE ffl/act.fs

VARIABLE PROFIT 
ACT-CREATE PLAN
ACT-CREATE ACTIONS

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

: TIME-DURATION>KEY ( t d -- k )
    SWAP 32 LSHIFT OR ;

: MASK ( n b -- n )
    -1 SWAP RSHIFT AND ;
 
: KEY>TIME-DURATION ( k -- t d )
    DUP 32 RSHIFT 
    SWAP 32 MASK ;

: INIT-ACTIONS 
    ACTIONS ACT-INIT ;

: ACTION! ( n k -- )
    ACTIONS ACT-INSERT ;

: {CASH} ( t -- )
    0 TIME-DURATION>KEY 
    0 SWAP ACTION! ;

: {RENT} ( t d p -- )
    -ROT TIME-DURATION>KEY DUP
    ACTIONS ACT-GET IF ROT MAX SWAP THEN
    ACTION! ;

: ADD-ORDER ( t d p -- )
    -ROT 2DUP + {CASH}
    ROT {RENT} ; 

: DO-ACTION ( n k -- )
    KEY>TIME-DURATION
    DUP 0= IF DROP CASH DROP 
         ELSE ROT  RENT THEN ;
' DO-ACTION CONSTANT EXEC

: COMPUTE-PROFIT ( -- )
    INIT
    EXEC ACTIONS ACT-EXECUTE ;
