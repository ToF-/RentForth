\ Rent.fs
INCLUDE ffl/hct.fs

20000 CONSTANT MAX-VALUES
MAX-VALUES HCT-CREATE VALUES 
VARIABLE MAXIMUM-VALUE
VARIABLE CURRENT-VALUE

: TIME>KEY ( n -- adr,cnt )
    BASE @ SWAP 0 64 BASE ! <# #S #> ROT BASE ! ;

: UPDATE-VALUE ( n,k -- )
    TIME>KEY 2DUP 2>R VALUES HCT-GET ( n,v,f )
    IF MAX THEN 2R> VALUES HCT-INSERT ;

: GET-VALUE ( k -- n,f | false )
    TIME>KEY VALUES HCT-GET ;

: UPDATE-MAX ( n adr -- )
    DUP @ ROT MAX SWAP ! ;

: INIT-VALUES ( -- )
    0 MAXIMUM-VALUE !
    0 CURRENT-VALUE ! ;

: ADD-ORDER ( start,duration,bid -- )
    ROT DUP GET-VALUE 0= IF 0 THEN MAXIMUM-VALUE UPDATE-MAX 
    ROT + SWAP MAXIMUM-VALUE @ + SWAP UPDATE-VALUE ;
