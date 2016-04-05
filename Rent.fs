\ Rent.fs

REQUIRE ffl/hct.fs
REQUIRE ffl/act.fs

10000 CONSTANT MAX-ORDERS#
0 CONSTANT CASH%
1 CONSTANT RENT%
MAX-ORDERS# HCT-CREATE PLAN 
VARIABLE HCT-PLAN
VARIABLE RENT-VALUE
ACT-CREATE ACTIONS

: INIT-PLAN 
    PLAN HCT-EMPTY? 0= IF
        MAX-ORDERS# PLAN HCT-INIT
    THEN ;

: INITIALIZE
    RENT-VALUE OFF
    INIT-PLAN ;
    

: TIME>STRING ( n -- addr c )
    S>D <# #S #> ;

: PLAN@ ( time -- n )
    TIME>STRING PLAN HCT-GET 0= IF 0 THEN ;

: PLAN! ( n time -- )
    TIME>STRING PLAN HCT-INSERT ;

: RENT ( time bid -- )
    RENT-VALUE @ +
    OVER PLAN@ MAX
    SWAP PLAN! ;

: CASH ( time )
    PLAN@ RENT-VALUE @ MAX RENT-VALUE ! ;

: DO-ACTION ( time bid type -- )
    ?DUP IF RENT ELSE CASH THEN ;

: ACTION>KEY ( start end bid -- k )
    ROT 21 LSHIFT ROT OR 21 LSHIFT SWAP OR ;

1 21 LSHIFT 1- CONSTANT 21MASK

: NEXT-FIELD ( k -- f k' )
    DUP 21MASK AND SWAP 21 RSHIFT ;

: KEY>ACTION ( k -- start end bid )
    NEXT-FIELD NEXT-FIELD SWAP ROT ;

: INSERT-ACTION ( start time bid -- )
    ACTION>KEY 0 SWAP ACTIONS ACT-INSERT ;

: ADD-ORDER ( start duration bid )
    >R 2DUP OVER 
    + R> INSERT-ACTION
    + 0 0  INSERT-ACTION ;

: EXECUTE-ACTION ( d k -- )
    SWAP DROP KEY>ACTION ROT DROP DO-ACTION ;

: COMPUTE-VALUE ( -- n )
    ['] EXECUTE-ACTION ACTIONS ACT-EXECUTE 
    RENT-VALUE @ ;
    
