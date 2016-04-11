\ Rent.fs

REQUIRE ffl/act.fs

VARIABLE PROFIT 
ACT-CREATE PLAN
ACT-CREATE ACTIONS
: INITIALIZE PLAN ACT-(FREE) ACTIONS ACT-(FREE) 0 PROFIT ! ;
: PLAN@ ( time -- value ) PLAN ACT-GET 0= IF 0 THEN ;
: PLAN! ( value time -- ) PLAN ACT-INSERT ;
: UPDATE-PROFIT ( time -- ) PLAN@ PROFIT @ MAX PROFIT ! ;
: RENT-AIRPLANE ( time duration price -- )
     -ROT + DUP PLAN@ ROT PROFIT @ + MAX SWAP PLAN! ;

21 CONSTANT LONG
17 CONSTANT SHORT

: <FIELD ( value cell #bits -- cell' ) LSHIFT OR ;

: MASK ( cell #bits -- cell' ) 1 SWAP LSHIFT 1- AND ;

: >FIELD ( cell #bits -- value cell' ) 2DUP MASK -ROT RSHIFT ; 

: ACTION>KEY ( time duration price -- key ) 
    ?DUP 0= IF + NIL NIL THEN
    SWAP ROT LONG <FIELD SHORT <FIELD ;

: KEY>ACTION ( key -- time duration price )
    SHORT >FIELD LONG >FIELD SWAP ROT ;

: ADD-ACTION ( key -- ) NIL SWAP ACTIONS ACT-INSERT ;

: ADD-ORDER ( time duration price -- ) 
    -ROT 2DUP 0 ACTION>KEY ADD-ACTION 
     ROT        ACTION>KEY ADD-ACTION ;

: EXEC-ACTION ( data key -- ) 
    KEY>ACTION ?DUP IF RENT-AIRPLANE 
    ELSE DROP UPDATE-PROFIT THEN DROP ;

: CALC-PROFIT ( -- )
    ['] EXEC-ACTION ACTIONS ACT-EXECUTE ;


