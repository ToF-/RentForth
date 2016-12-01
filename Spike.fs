
\ Rent.fs
\ usage example :
\ INIT
\ 0 5 100 ORDER
\ 3 7 140 ORDER
\ 5 9  80 ORDER
\ 6 9  70 ORDER
\ COMPUTE-PROFIT
\ PROFIT ?

\ should display: 180

REQUIRE ffl/act.fs

ACT-CREATE PLAN
ACT-CREATE ACTIONS
VARIABLE PROFIT 

: PLAN@ ( t -- n   finds profit at time t or 0 )
	PLAN ACT-GET
	0= IF 0 THEN ;
	 
: PLAN! ( n t --   stores profit at time t )
	PLAN ACT-INSERT ;

: UPDATE-PROFIT ( n -- update value with n if n is greater )
	PROFIT @ 
	MAX
	PROFIT ! ;

: UPDATE-PLAN ( n t -- update profit at time t with n if n is greater )
	DUP PLAN@ 
	ROT MAX 
	SWAP PLAN! ;

: CASH ( t --   update value with profit at time t if greater )
	PLAN@ 
	UPDATE-PROFIT ;

: RENT ( t d n  -- cash profit at time t, then update profit at t+d with PROFIT+p )
	ROT DUP CASH
	SWAP PROFIT @ +
	-ROT + 
	UPDATE-PLAN ;

: MASK ( b -- m  creates a mask of 32 bits all set to 1 ) 
	-1 SWAP RSHIFT ;

: ACTION>KEY ( t d -- k   encode time and duration in a word value )
	SWAP 32 LSHIFT OR ;

: KEY>ACTION ( k -- t d   decode time and duration from a word value )
	DUP 32 RSHIFT 
	SWAP 32 MASK AND ; 

: {CASH} ( t -- store a cash action event in the action tree )
	0 ACTION>KEY
	0 SWAP
	ACTIONS ACT-INSERT ;

: {RENT} ( t d p -- store/update a rent action event in the action tree )
	-ROT
	ACTION>KEY DUP 
	ACTIONS ACT-GET IF ROT MAX SWAP THEN 
	ACTIONS ACT-INSERT ;

: ORDER ( t d p -- store cash and rent actions for order t d p )
	-ROT 2DUP + {CASH}
	ROT {RENT} ;

: CASH? ( d -- b  return true if action is Cash ie duration d is 0, false if Rent )
	0= ;

: ACTION ( n k -- perform action defined by key and value )
	KEY>ACTION 
	DUP CASH? 
	IF  DROP CASH DROP
	ELSE ROT RENT THEN ;

' ACTION CONSTANT EXEC-ACTION

: COMPUTE-PROFIT ( compute the profit value for all given orders )
	0 PROFIT !
	PLAN ACT-INIT
	EXEC-ACTION ACTIONS ACT-EXECUTE ; 

: INIT ( -- initialize the action tree )
	ACTIONS ACT-INIT ;



