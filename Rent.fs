\ Rent.fs
REQUIRE ffl/act.fs
REQUIRE ffl/hct.fs

VARIABLE EVENT-ID

10000 CONSTANT MAX-ORDERS
0  CONSTANT CASH%
1  CONSTANT RENT%
1  CONSTANT KIND%
15 CONSTANT KEY%
24 CONSTANT TIME%
0  CONSTANT NO-DATA
1 TIME% LSHIFT 1- CONSTANT TIME-MASK%

VARIABLE ACT-EVENTS
VARIABLE RENT-VALUE
VARIABLE HCT-VALUES

: EVENTS ( -- act )
    ACT-EVENTS  @ ;

: VALUES ( -- hct )
    HCT-VALUES @ ;


: NEXT-ID ( -- id ) EVENT-ID DUP @ 1 ROT +! ;

: SHIFTOR LSHIFT OR ;

: EVENT-KEY ( kind time -- key ) 
    KIND% SHIFTOR NEXT-ID SWAP KEY% SHIFTOR ;

: RENT-DATA ( bid start duration -- data )
    + TIME% SHIFTOR ;

: RENT-EVENT ( start duration bid -- data key )
    ROT DUP >R ROT RENT-DATA RENT% R> EVENT-KEY ;

: CASH-EVENT ( start -- data key )
    NO-DATA CASH% ROT EVENT-KEY ;

: ADD-ORDER ( start duration bid -- )
    >R 2DUP R> RENT-EVENT EVENTS ACT-INSERT
    + CASH-EVENT EVENTS ACT-INSERT ;

: INIT-EVENTS
    EVENTS ?DUP IF ACT-(FREE) THEN ACT-NEW ACT-EVENTS ! ;

: INIT-VALUES
    HCT-VALUES @ ?DUP IF HCT-(FREE) THEN MAX-ORDERS HCT-NEW HCT-VALUES ! ;

: INITIALIZE 
    EVENT-ID OFF RENT-VALUE OFF INIT-EVENTS INIT-VALUES ;

: TIME-KEY ( time -- addr c ) 
    S>D <# #S #> ;

: GET-VALUE ( time -- n )
    TIME-KEY HCT-VALUES @ HCT-GET 0= IF 0 THEN ;

: SET-VALUE ( n time -- )
    TIME-KEY VALUES HCT-INSERT ;

: RENT ( start end bid -- )
    ROT GET-VALUE RENT-VALUE @ MAX + 
    OVER GET-VALUE MAX SWAP SET-VALUE ;

: CASH ( start -- )
    GET-VALUE RENT-VALUE @ MAX RENT-VALUE ! ;

: EVENT ( start [end bid] kind -- )
    RENT% = IF ['] RENT ELSE ['] CASH THEN 
    EXECUTE ;

: EVENT-KIND ( key -- kind ) 
    KIND% AND ;

: TIME ( data -- time )
    TIME-MASK% AND ;
    
: GET-EVENT ( data key -- start [end bid] kind )
    KEY% RSHIFT
    DUP EVENT-KIND >R 
    KIND% RSHIFT
    SWAP R@ RENT% = IF 
        DUP TIME 
        SWAP TIME% RSHIFT 
        SWAP 
    ELSE DROP THEN R> ;

: RUN-EVENT ( data key -- )
    GET-EVENT EVENT ;
    
: COMPUTE-VALUE
    ['] RUN-EVENT EVENTS ACT-EXECUTE ;
    

