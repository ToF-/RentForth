\ Rent.fs

REQUIRE ffl/act.fs

VARIABLE PROFIT 
ACT-CREATE PLAN
ACT-CREATE ACTIONS
: INITIALIZE PLAN ACT-(FREE) ACTIONS ACT-(FREE) 0 PROFIT ! ;
: PLAN@ ( time -- value ) PLAN ACT-GET 0= IF 0 THEN ;
: PLAN! ( value time -- ) PLAN ACT-INSERT ;
: UPDATE-PROFIT ( time -- ) PLAN@ PROFIT @ MAX PROFIT ! ;
: RENT ( time duration price -- ) -ROT + DUP PLAN@ ROT PROFIT @ + MAX SWAP PLAN! ;

21 CONSTANT LONG
17 CONSTANT SHORT

: <FIELD ( value cell #bits -- cell' ) LSHIFT OR ;

: MASK ( cell #bits -- cell' ) 1 SWAP LSHIFT 1- AND ;

: >FIELD ( cell #bits -- value cell' ) 2DUP MASK -ROT RSHIFT ; 

: ACTION>KEY ( time duration price -- key ) 
    ?DUP 0= IF + 0 0 THEN
    SWAP ROT LONG <FIELD SHORT <FIELD ;

: KEY>ACTION ( key -- time duration price ) SHORT >FIELD LONG >FIELD SWAP ROT ;


: INSERT ( key -- )
    0 SWAP ACTIONS ACT-INSERT ;

: ADD-ORDER ( time duration price -- ) 
    -ROT 2DUP 0 ACTION>KEY INSERT ROT ACTION>KEY INSERT ;

: EXEC-ACTION ( data key -- )
    KEY>ACTION ?DUP IF RENT ELSE DROP UPDATE-PROFIT THEN DROP ;

: .ACTION ( data key -- )
    NIP KEY>ACTION CR . . . ;

: CALC-PROFIT ( -- )
    ['] EXEC-ACTION ACTIONS ACT-EXECUTE ;


