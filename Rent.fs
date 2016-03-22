\ Rent.fs
INCLUDE ffl/hct.fs

20000 CONSTANT MAX-VALUES
MAX-VALUES HCT-CREATE VALUE-TABLE 
VARIABLE MAX-VALUE

: S>KEY ( n -- adr,cnt )
    BASE @ SWAP 0 64 BASE ! <# #S #> ROT BASE ! ;

: UPDATE-VALUE ( n,k -- )
    S>KEY 2DUP 2>R VALUE-TABLE HCT-GET ( n,v,f )
    IF MAX THEN 2R> VALUE-TABLE HCT-INSERT ;

: GET-VALUE ( k -- n,f | false )
    S>KEY VALUE-TABLE HCT-GET ;

: UPDATE-MAX ( n adr -- )
    DUP @ ROT MAX SWAP ! ;

: INIT-VALUES ( -- )
    0 MAX-VALUE ! ;

: ADD-ORDER ( start,duration,bid -- )
    MAX-VALUE UPDATE-MAX 2DROP ;
