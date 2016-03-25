\ Rent.fs
INCLUDE ffl/hct.fs

10000 CONSTANT MAX-ORDERS
VARIABLE VALUES
VARIABLE CUR-VALUE
VARIABLE MAX-VALUE

: MAX! ( n adr -- )
    DUP @ ROT MAX SWAP ! ;

: S>KEY ( n -- adr c )
    S>D <# #S #> ;

: VALUE#@ ( time -- n )
    S>KEY VALUES @ HCT-GET 0= IF 0 THEN ;

: VALUE#! ( time -- n )
    S>KEY VALUES @ HCT-INSERT ;

: VALUE#-MAX! ( n time -- )
    DUP VALUE#@ ROT MAX SWAP VALUE#! ;

: INITIALIZE
    0 CUR-VALUE ! 0 MAX-VALUE !
    VALUES @ ?DUP IF HCT-FREE THEN
    MAX-ORDERS 2* HCT-NEW VALUES ! ;
    
: ADD-ORDER ( s d b -- )
    -ROT OVER VALUE#@ CUR-VALUE MAX! \ b s d 
    OVER CUR-VALUE @ SWAP VALUE#-MAX! \ b s d 
    + DUP ROT CUR-VALUE @ +  SWAP VALUE#-MAX!
    VALUE#@ MAX-VALUE MAX! ; 
    

