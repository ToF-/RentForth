\ Rent.fs
INCLUDE ffl/hct.fs

10000 CONSTANT MAX-ORDERS
VARIABLE VALUES

: MAX! ( n adr -- )
    DUP @ ROT MAX SWAP ! ;

: S>KEY ( n -- adr c )
    BASE @ 64 BASE ! 
    SWAP S>D <# #S #>
    ROT BASE ! ;

: VALUE#@ ( time -- n )
    S>KEY VALUES @ HCT-GET 0= IF 0 THEN ;

: VALUE#! ( time -- n )
    S>KEY VALUES @ HCT-INSERT ;

: VALUE#-MAX! ( n time -- )
    DUP VALUE#@ ROT MAX SWAP VALUE#! ;

: INITIALIZE
    VALUES @ ?DUP IF HCT-FREE THEN
    MAX-ORDERS 2* HCT-NEW VALUES ! ;
    


