\ Rent.fs
REQUIRE ffl/hct.fs

VARIABLE PROFIT 
10000 CONSTANT MAXORDER#
MAXORDER# HCT-CREATE PLAN

: INITIALIZE
    MAXORDER# PLAN HCT-INIT
    0 PROFIT ! ;

: INT>STR ( n -- addr # )
    S>D <# #S #> ;

: PLAN# ( time -- addr # plan )
   INT>STR PLAN ; 

: PLAN@ ( time -- n  )
    PLAN# HCT-GET 0= IF 0 THEN PROFIT @ MAX ;
 
: PLAN! ( n time -- )
    DUP PLAN@ ROT MAX SWAP PLAN# HCT-INSERT ;
 
: CASH ( time -- )
    PLAN@ PROFIT ! ;
      
: RENT ( end price -- )
    PROFIT @ + SWAP PLAN! ;
    
: <<FIELD ( n cell #bits -- cell' )
    LSHIFT OR ;

: <<TYPE ( f cell -- cell' )
    1 <<FIELD ;

: <<TIME ( t cell -- cell' )
    21 <<FIELD ;

: <<PRICE ( p cell -- cell' )
    17 <<FIELD ;

0 CONSTANT CASH%
1 CONSTANT RENT%

: ACTION ( price end time type -- action )
    SWAP <<TYPE <<TIME <<PRICE ;

: RENT-PARAMS ( start duration price -- price end start )
    -ROT OVER + SWAP ;

: CASH-PARAMS ( start duration -- 0 0 end )
    + 0 0 ROT ; 

: ORDER-ACTIONS ( start duration price -- action action )
    >R 2DUP R>
    RENT-PARAMS RENT% ACTION >R
    CASH-PARAMS CASH% ACTION R> ;
    

