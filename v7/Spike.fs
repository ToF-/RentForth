
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

    BYE
