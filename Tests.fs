\ Tests.fs  gforth Tests.fs

INCLUDE Rent.fs
INCLUDE QSort.fs

: EQUALS ( n1,n2 -- f with message if false )
    2DUP <> IF ." EXPECTED:" . ." BUT WAS:" . FALSE
    ELSE 2DROP TRUE THEN ;
    
: TEST-EQUALS
    ASSERT( 2 2 + 4 EQUALS ) ;

: TEST-MID
    ASSERT( HERE DUP 10 CELLS + MID HERE 5 CELLS + EQUALS ) 
    ASSERT( HERE DUP 1  CELLS + MID HERE 0 CELLS + EQUALS )
    ASSERT( HERE DUP 2  CELLS + MID HERE 1 CELLS + EQUALS ) ;

CREATE ARRAY 4807 ,   23 ,  110 , 2367 ,  365 , 
               12 , 9920 ,  876 ,    5 , 3402 ,
             2644 ,  110 ,    0 , 9999 ,  456 ,
             8463 ,   76 ,  823 ,   19 ,   47 ,


: TEST-EXCHANGE 
    ASSERT( ARRAY 0 CELLS + ARRAY 1 CELLS + EXCHANGE
            ARRAY 0 CELLS + @ 23   EQUALS 
            ARRAY 1 CELLS + @ 4807 EQUALS AND ) ;

: .ARRAY 20 0 DO ARRAY I CELLS + @ . LOOP ;

: TEST-PARTITION
  ARRAY 10 CELLS + @
  ARRAY DUP 20 CELLS + CELL- PARTITION 2DROP 2DROP
  ARRAY DUP 10 CELLS + SWAP DO I @ OVER ASSERT( < ) CELL +LOOP  
  ARRAY DUP 20 CELLS + SWAP 15 CELLS + DO
     I @ OVER ASSERT( > ) CELL +LOOP
  DROP
;
    
: TEST-QSORT
    ARRAY DUP 19 CELLS + QSORT
    ARRAY DUP 19 CELLS + SWAP DO
        I @ I CELL+ @ ASSERT( <= ) CELL +LOOP
;

: TEST-SORT
    ARRAY 1 SORT
    ARRAY 20 SORT
    ARRAY DUP 19 CELLS + SWAP DO
        I @ I CELL+ @ ASSERT( <= ) CELL +LOOP ;

CREATE SRCE 6 ALLOT
CREATE DEST 6 ALLOT

: IND-INF ( adr,adr' -- f )
    SWAP C@ SWAP C@ < ;
' IND-INF IS INF

: TEST-ALT-INF
    S" FooBar" SRCE SWAP CMOVE
    6 0 DO SRCE I + ARRAY I CELLS + ! LOOP
    ARRAY 6 SORT
    6 0 DO ARRAY I CELLS + @ C@ DEST I + C! LOOP
    S" BFaoor" DEST 6 ASSERT( COMPARE 0 EQUALS )
;


PAGE
' < IS INF
TEST-EQUALS
TEST-MID
TEST-EXCHANGE
TEST-PARTITION
TEST-QSORT
TEST-SORT
' IND-INF IS INF
TEST-ALT-INF

." Success "
.S
BYE
