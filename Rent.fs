\ Rent.fs

10000 CONSTANT MAX-ORDERS
VARIABLE CURRENT-VALUE
VARIABLE RENT-VALUE
VARIABLE RENT-HCT-VALUES

: RENT-VALUES
    RENT-HCT-VALUES @ ;

: INIT-RENT
    0 RENT-VALUE ! 0 CURRENT-VALUE !
    RENT-VALUES ?DUP IF HCT-FREE THEN
    MAX-ORDERS 2 * HCT-NEW RENT-HCT-VALUES ! ;

: S>KEY ( n -- addr c )
    S>D <# #S #> ;

: RENT-VALUE@ ( t -- n )
    S>KEY RENT-VALUES HCT-GET 0= IF 0 THEN ;

: RENT ( s d b -- )
    CURRENT-VALUE @ + 
    DUP RENT-VALUE @ MAX RENT-VALUE !
    -ROT + S>KEY RENT-VALUES HCT-INSERT ;

: COLLECT ( t -- )
    RENT-VALUE@ CURRENT-VALUE @ MAX CURRENT-VALUE ! ;
