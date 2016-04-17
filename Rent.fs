\ Rent.fs

REQUIRE ffl/act.fs

VARIABLE PROFIT 
ACT-CREATE PLAN
ACT-CREATE ACTIONS
: INITIALIZE PLAN ACT-(FREE) ACTIONS ACT-(FREE) 0 PROFIT ! ;
: PLAN@ ( time -- value ) PLAN ACT-GET 0= IF 0 THEN ;
: PLAN! ( value time -- ) PLAN ACT-INSERT ;
: UPDATE-PROFIT ( time -- ) PLAN@ PROFIT @ MAX PROFIT ! ;
: UPDATE-PLAN ( value time -- ) DUP PLAN@ ROT MAX SWAP PLAN! ;
: RENT-AIRPLANE ( time duration price -- )
    ROT DUP UPDATE-PROFIT -ROT
    ?DUP IF PROFIT @ +  -ROT + UPDATE-PLAN ELSE 2DROP THEN ;

: <FIELD ( value cell #bits -- cell' ) LSHIFT OR ;

21 CONSTANT LONG
17 CONSTANT SHORT

: (ACTION-KEY) ( time duration price -- key )
    SWAP ROT LONG <FIELD SHORT <FIELD ;

: ACTION>KEY ( time duration price -- key )
    ?DUP 0= IF + 0 0 THEN (ACTION-KEY) ;

: MASK ( #bits -- mask ) -1 SWAP LSHIFT INVERT ;

: >FIELD ( cell #bits -- value cell' ) 2DUP MASK AND -ROT RSHIFT ; 

: KEY>ACTION ( key -- time duration price )
    SHORT >FIELD LONG >FIELD SWAP ROT ;

: ADD-ACTION ( key -- ) NIL SWAP ACTIONS ACT-INSERT ;

: ADD-ORDER ( time duration price -- ) 
    -ROT 2DUP 0 ACTION>KEY ADD-ACTION 
     ROT        ACTION>KEY ADD-ACTION ;

: EXEC-ACTION ( data key -- ) 
    KEY>ACTION RENT-AIRPLANE DROP ;

: CALC-PROFIT ( -- )
    ['] EXEC-ACTION ACTIONS ACT-EXECUTE ;


