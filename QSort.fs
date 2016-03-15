\ QSort

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
    DUP 2 < IF 2DROP EXIT THEN
    1- CELLS OVER + QSORT ;

