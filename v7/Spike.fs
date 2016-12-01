
	REQUIRE ffl/act.fs

	ACT-CREATE PROFIT

    : PROFIT@ ( t -- n   finds profit at time t or 0 )
        PROFIT ACT-GET
        0= IF 0 THEN ;
         
    : PROFIT! ( n t --   stores profit at time t )
        PROFIT ACT-INSERT ;

    PROFIT ACT-INIT
	500000 PROFIT@ .
	4807 500000 PROFIT!
	500000 PROFIT@ .

    VARIABLE V 

    : UPDATE-V ( n -- update value with n if n is greater )
        V @ 
        MAX
        V ! ;

    : UPDATE-PROFIT ( n t -- update profit at time t with n if n is greater )
        DUP PROFIT@ 
        ROT MAX 
        SWAP PROFIT! ;

	: CASH ( t --   update value with profit at time t if greater )
		PROFIT@ 
        UPDATE-V ;

    : RENT ( t d n  -- cash profit at time t, then update profit at t+d with V+p )
        ROT DUP CASH
        SWAP V @ +
        -ROT + 
        UPDATE-PROFIT ;

    0 V !

    0 5 100 RENT
    3 7 140 RENT
    5       CASH
    5 9 80  RENT
    6 9 70  RENT
    10      CASH
    14      CASH
    15      CASH
    
    V ?

    : MASK ( b -- m  creates a mask of 32 bits all set to 1 ) 
        -1 SWAP RSHIFT ;

    : ACTION>KEY ( t d -- k   encode time and duration in a word value )
        SWAP 32 LSHIFT OR ;

    : KEY>ACTION ( k -- t d   decode time and duration from a word value )
        DUP 32 RSHIFT 
        SWAP 32 MASK AND ; 

    ACT-CREATE ACTIONS
 
    : {CASH} ( t -- stores a cash action event in the action list )
        0 ACTION>KEY
        0 SWAP
        ACTIONS ACT-INSERT ;

    : {RENT} ( t d p -- stores a rent action event in the action list )
        -ROT
        ACTION>KEY
        ACTIONS ACT-INSERT ;

    : .ACTION ( n k -- pretty print an action read in the action list )
        KEY>ACTION CR
        DUP 0= IF DROP . ." Cash " DROP 
        ELSE SWAP . . . ." Rent " THEN ; 

    ACTIONS ACT-INIT
    5 9 100 {RENT}
    3 7 140 {RENT}
    5 {CASH}
    3 {CASH}

    ' .ACTION ACTIONS ACT-EXECUTE
    BYE
