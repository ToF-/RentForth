\ Rent.fs

REQUIRE ffl/hct.fs

VARIABLE HCT-PLAN
VARIABLE RENT-VALUE

: INITIALIZE
    RENT-VALUE OFF
    HCT-PLAN @ ?DUP IF HCT-(FREE) THEN
    10000 HCT-NEW HCT-PLAN ! ;

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

    
