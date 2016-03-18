\ QSort

: NOT 0= ;
: -CELL -1 CELLS ;
: CELL- 1 CELLS - ;

: MID ( l,h -- m )
    OVER - 2 / -CELL AND + ;

: EXCHANGE ( add1,add2 ) 
    DUP @ -ROT OVER @ SWAP ! ! ;

DEFER  INF 
' < IS INF

: PARTITION ( l,h -- l,h',l',h )
    2DUP MID @ >R  \ pivot
    2DUP BEGIN
        SWAP BEGIN  DUP @ R@ INF WHILE CELL+ REPEAT
        SWAP BEGIN R@ OVER @ INF WHILE CELL- REPEAT
        2DUP <= IF 2DUP EXCHANGE SWAP CELL+ SWAP CELL- THEN
    2DUP > UNTIL R> DROP SWAP ROT ;

: LIMITS ( adr,len -- l,h )
    1- CELLS OVER + ;

: QSORT ( l,h -- )
    PARTITION 
    2DUP < IF RECURSE ELSE 2DROP THEN
    2DUP < IF RECURSE ELSE 2DROP THEN ;

: SORT ( adr,len -- )
    DUP 1 > IF LIMITS QSORT ELSE 2DROP THEN ;


DEFER CMP
' - IS CMP

: (LOOK-UP) ( elem,l,h -- adr | f  )
    ROT >R BEGIN
        2DUP > NOT 
    WHILE 
        2DUP MID DUP @ R@ CMP
        DUP 0= IF ( found ) 
        ELSE 0 < IF CELL+ ROT DROP SWAP
        ELSE        CELL- NIP THEN THEN 
    REPEAT R> DROP
    IF DROP FALSE ELSE -ROT 2DROP THEN ;

: LOOK-UP ( elem,adr,len -- )
    ?DUP IF LIMITS (LOOK-UP) ELSE 2DROP FALSE THEN ;
    

