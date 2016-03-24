\ Rent.fs
INCLUDE ffl/hnt.fs
INCLUDE ffl/hct.fs

10000 CONSTANT MAX-ORDERS
VARIABLE VALUES 0 VALUES !
VARIABLE MAXIMUM
VARIABLE CURRENT

: UPDATE-VARIABLE ( n adr -- )
    DUP @ ROT MAX SWAP ! ;

: S>KEY ( n -- adr,cnt )
    BASE @ SWAP 64 BASE !
    S>D <# #S #> 
    ROT BASE ! ;

: GET-VALUE ( k -- n,f | false )
    S>KEY VALUES @ HCT-GET 0= IF 0 THEN ;

: SET-VALUE ( n k -- )
    S>KEY VALUES @ HCT-INSERT ;

: UPDATE-VALUE ( n,k -- )
    TUCK GET-VALUE 
    MAX 
    SWAP SET-VALUE ;
    
: FREE-VALUES
    VALUES @ ?DUP IF HCT-FREE THEN ;

: NEW-VALUES
    MAX-ORDERS 2* HCT-NEW VALUES ! ;

: INITIALIZE ( -- )
    FREE-VALUES NEW-VALUES
    0 MAXIMUM ! 0 CURRENT ! ;

: UPDATE-VALUE-WITH-VARIABLE ( k addr -- )
    @ SWAP UPDATE-VALUE ;

: UPDATE-VARIABLE-WITH-VALUE ( k addr -- )
    GET-VALUE SWAP UPDATE-VARIABLE ;

: ADD-ORDER ( start,duration,bid -- )
    ROT DUP CURRENT UPDATE-VALUE-WITH-VARIABLE 
    CURRENT OVER UPDATE-VARIABLE-WITH-VALUE
    SWAP CURRENT @ + MAXIMUM UPDATE-VARIABLE
    + DUP MAXIMUM UPDATE-VALUE-WITH-VARIABLE
    GET-VALUE MAXIMUM UPDATE-VARIABLE ;

    

