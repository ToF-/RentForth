\ Rent.fs

REQUIRE ffl/hct.fs
REQUIRE ffl/act.fs

10000 CONSTANT MAX-ORDERS#
0 CONSTANT CASH%
1 CONSTANT RENT%
VARIABLE HCT-PLAN
VARIABLE RENT-VALUE
VARIABLE EVENT-ID
VARIABLE EVENTS

: INITIALIZE
    RENT-VALUE OFF EVENT-ID OFF
    HCT-PLAN @ ?DUP IF HCT-(FREE) THEN
    MAX-ORDERS# HCT-NEW HCT-PLAN ! 
    EVENTS @ ?DUP IF ACT-(FREE) THEN
    ACT-NEW EVENTS ! ;

: TIME>STRING ( n -- addr c )
    S>D <# #S #> ;

: PLAN@ ( time -- n )
    TIME>STRING HCT-PLAN @ HCT-GET 0= IF 0 THEN ;

: PLAN! ( n time -- )
    TIME>STRING HCT-PLAN @ HCT-INSERT ;

: RENT ( time bid -- )
    RENT-VALUE @ +
    OVER PLAN@ MAX
    SWAP PLAN! ;

: CASH ( time )
    PLAN@ RENT-VALUE @ MAX RENT-VALUE ! ;

: RUN-EVENT ( time bid type -- )
    ?DUP IF  RENT ELSE CASH THEN ;

: NEXT-ID ( -- id )
    EVENT-ID DUP @ 1 ROT +! ;

: EVENT-KEY ( time type -- key )
    SWAP 1 LSHIFT OR 15 LSHIFT NEXT-ID OR ;

: RENT-DATA ( time bid -- data )
    SWAP 24 LSHIFT OR ;
    
: CASH-DATA ( -- 0 )
    0 ;

: RENT-EVENT ( start duration bid -- data key )
    -ROT OVER + ROT RENT-DATA
    RENT% EVENT-KEY ;

: CASH-EVENT ( start duration -- data key )
    CASH-DATA -ROT
    + CASH% EVENT-KEY ;

: ADD-EVENT ( data key -- )
    EVENTS @ ACT-INSERT ;

: ADD-ORDER ( start duration bid -- )
    >R 2DUP R> 
    RENT-EVENT ADD-EVENT
    CASH-EVENT ADD-EVENT ;

: GET-EVENT ( key data -- time [bid] type )
    SWAP 16 RSHIFT DUP 1 AND
    DUP 1 16 LSHIFT 1- AND SWAP
    16 RSHIFT DUP 1 AND SWAP
    1 RSHIFT -ROT ;

