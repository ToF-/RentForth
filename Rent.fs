\ Rent.fs
INCLUDE ffl/hct.fs

10000 CONSTANT MAX-ORDERS
VARIABLE VALUES
VARIABLE CUR-VALUE
VARIABLE MAX-VALUE
VARIABLE LAST-END

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
    0 CUR-VALUE ! 0 MAX-VALUE ! 0 LAST-END !
    VALUES @ ?DUP IF HCT-FREE THEN
    MAX-ORDERS 2* HCT-NEW VALUES ! ;
    
: pr
    values @ hct-dump
    cr cur-value ? max-value ?
    key drop
;
: END-TO-START ( s )
    LAST-END @ VALUE#@ SWAP VALUE#-MAX! ;

: ADD-ORDER ( s d b -- )
    -ROT OVER DUP LAST-END @ > IF DUP END-TO-START THEN
         VALUE#@ CUR-VALUE MAX! \ b s d 
    OVER CUR-VALUE @ SWAP VALUE#-MAX! \ b s d 
    + DUP ROT CUR-VALUE @ +  SWAP DUP LAST-END ! VALUE#-MAX!
    VALUE#@ MAX-VALUE MAX! ; 
    

