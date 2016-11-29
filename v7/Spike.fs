
	VARIABLE RENT-VALUE
	0 RENT-VALUE !

    CREATE PROFIT 200 CELLS ALLOT
    PROFIT 200 CELLS ERASE   

    : PROFIT@ ( t -- m   finds profit value at time t or 0 )
        CELLS PROFIT + @ ;
         
    : PROFIT! ( m t --   stores profit value at time t )
        CELLS PROFIT + ! ;

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
         
    0 5 100 UPDATE-AND-RENT
    3 7 140 UPDATE-AND-RENT
    5       UPDATE-VALUE
    5 9 80  UPDATE-AND-RENT
    6 9 70  UPDATE-AND-RENT
    10      UPDATE-VALUE
    14      UPDATE-VALUE
    15      UPDATE-VALUE
    RENT-VALUE ?
    BYE
    
