    REQUIRE ffl/act.fs

	VARIABLE VALUE
	0 VALUE !

    ACT-CREATE PROFIT

    : PROFIT@ ( t -- m   finds profit value at time t or 0 )
        PROFIT ACT-GET 0= IF 0 THEN ;
         
    : PROFIT! ( m t --   stores profit value at time t )
        PROFIT ACT-INSERT ;

	: CASH ( t --   update value with profit at time t if greater )
		PROFIT@ VALUE @ MAX
		VALUE ! ;

    : UPDATE-PROFIT ( p t -- update profit at time t with p if p is greater )
        DUP PROFIT@ 
        ROT MAX 
        SWAP PROFIT! ;
         
    : RENT ( s d p  -- update profit table at s+d for price p )
        ROT DUP CASH
        SWAP VALUE @ +
        -ROT + UPDATE-PROFIT ;
         
    : ACTION>KEY ( t d -- k   encode time and duration in a word value )
        SWAP 32 LSHIFT OR ;

    : MASK ( b -- m  creates a mask of 32 bits all set to 1 ) 
        -1 SWAP RSHIFT ;

    : KEY>ACTION ( k -- t d   decode time and duration from a word value )
        DUP 32 RSHIFT 
        SWAP 32 MASK AND ; 

    ACT-CREATE ACTIONS

    : ACTION@ ( k -- p   retrieve action from k or 0 if not found )
        ACTIONS ACT-GET 0= IF 0 THEN ;

    : ACTION! ( d k --   insert action with duration d at key k )
        ACTIONS ACT-INSERT ;

    : {RENT} ( s d p -- record rent action at key s|d if not already in tree with greater p )
        -ROT ACTION>KEY DUP ACTION@
        ROT MAX SWAP ACTION!  ;

    : {CASH} ( s d -- record update action at key s+d|0 with value 0 )
        + 0 ACTION>KEY 0 SWAP ACTION! ;

    0 5 100 {RENT}
    5 0     {CASH}
    3 7 140 {RENT}
    10 0    {CASH}
    3 7 120 {RENT}
    10 0    {CASH}
    5 9 80  {RENT}
    14 0    {CASH}
    6 9 70  {RENT}
    15 0    {CASH}

    : .ACTION ( p k -- pretty print action )
        KEY>ACTION SWAP ." at " . 
        DUP 0= IF ." CASH " 2DROP ELSE ." RENT " . . THEN CR ;

    ' .ACTION ACTIONS ACT-EXECUTE

    0 5 100 RENT
    3 7 140 RENT
    5       CASH
    5 9 80  RENT
    6 9 70  RENT
    10      CASH
    14      CASH
    15      CASH
    VALUE ? CR

	4807 500000 PROFIT! 
    : .PROFIT-NODE ( m t --   pretty print the values )
        ." Profit[ " . ." ] = " . CR ;

    ' .PROFIT-NODE PROFIT ACT-EXECUTE 
    BYE
    
