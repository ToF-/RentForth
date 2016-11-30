    REQUIRE ffl/act.fs

	VARIABLE RENT-VALUE
	0 RENT-VALUE !

    ACT-CREATE PROFIT

    : PROFIT@ ( t -- m   finds profit value at time t or 0 )
        PROFIT ACT-GET 0= IF 0 THEN ;
         
    : PROFIT! ( m t --   stores profit value at time t )
        PROFIT ACT-INSERT ;

	: UPDATE-VALUE ( t --   update value with profit at time t if greater )
		PROFIT@ RENT-VALUE @ MAX
		RENT-VALUE ! ;

    : UPDATE-PROFIT ( p t -- update profit at time t with p if p is greater )
        DUP PROFIT@ 
        ROT MAX 
        SWAP PROFIT! ;
         
    : UPDATE-AND-RENT ( s d p  -- update profit table at s+d for price p )
        ROT DUP UPDATE-VALUE
        SWAP RENT-VALUE @ +
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

    : RENT-ACTION ( s d p -- record rent action at key s|d if not already in tree with greater p )
        -ROT ACTION>KEY DUP ACTION@
        ROT MAX SWAP ACTION!  ;

    : UPDATE-ACTION ( s d -- record update action at key s+d|0 with value 0 )
        + 0 ACTION>KEY 0 SWAP ACTION! ;

    0 5 100 RENT-ACTION
    5 0     UPDATE-ACTION
    3 7 140 RENT-ACTION
    10 0    UPDATE-ACTION
    3 7 120 RENT-ACTION
    10 0    UPDATE-ACTION
    5 9 80  RENT-ACTION
    14 0    UPDATE-ACTION
    6 9 70  RENT-ACTION
    15 0    UPDATE-ACTION

    : .ACTION ( p k -- pretty print action )
        KEY>ACTION SWAP ." Time:" . 
        DUP 0= IF ." UPDATE " 2DROP ELSE ." RENT " ." Duration:" . ." Price:" . THEN CR ;

    ' .ACTION ACTIONS ACT-EXECUTE
    BYE

    0 5 100 UPDATE-AND-RENT
    3 7 140 UPDATE-AND-RENT
    5       UPDATE-VALUE
    5 9 80  UPDATE-AND-RENT
    6 9 70  UPDATE-AND-RENT
    10      UPDATE-VALUE
    14      UPDATE-VALUE
    15      UPDATE-VALUE
    RENT-VALUE ? CR

	4807 500000 PROFIT! 
    : .PROFIT-NODE ( m t --   pretty print the values )
        ." Profit[ " . ." ] = " . CR ;

    ' .PROFIT-NODE PROFIT ACT-EXECUTE 
    BYE
    
