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

: QSORT ( l,h -- )
    PARTITION 
    2DUP < IF RECURSE ELSE 2DROP THEN
    2DUP < IF RECURSE ELSE 2DROP THEN ;

: SORT ( adr,len -- )
    1- ?DUP IF CELLS OVER + QSORT ELSE DROP THEN ;


DEFER CMP
' - IS CMP

: (LOOK-UP) ( elem,l,h -- adr,t | f  )
    FALSE >R ROT >R
    BEGIN
        2DUP > NOT 
    WHILE  ( l,h -- )
        2DUP MID DUP @ R@ CMP ( l,h,m,f -- )
        ?DUP 0= IF R> R> 2DROP TRUE >R >R 
        ELSE 0 < IF  ( l,h,m -- )
            CELL+ ROT DROP SWAP ( m,h -- )
        ELSE ( l,h,m -- )
            CELL- NIP
        THEN THEN
    REPEAT ( l,h -- )
    2DROP R> R> DUP 0= IF NIP THEN ;


: LOOK-UP ( elem,adr,len -- )
    ?DUP IF 1- CELLS OVER + (LOOK-UP) ELSE 2DROP FALSE THEN ;
    

