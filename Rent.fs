\ Rent.fs

\ example

\    INITIALIZE
\    6 9  70 ADD-ORDER
\    5 9  80 ADD-ORDER
\    3 7 140 ADD-ORDER
\    0 5 100 ADD-ORDER
\    CALC-RENT-VALUE
\    RENT-VALUE ?

\ act = AVL binary tree cell module
REQUIRE ffl/act.fs 

ACT-CREATE PROFITS  \ store profit value at a given time 
ACT-CREATE OPERATIONS \ store operations ordered by time 

\ retrieve profit at a given time or 0
: PROFIT@ ( t -- n )
    PROFITS ACT-GET 0= IF 0 THEN ;

\ store profit at a given time
: PROFIT! ( n t -- ) 
    PROFITS ACT-INSERT ;

VARIABLE RENT-VALUE 

: INITIALIZE          
    PROFITS    ACT-(FREE) 
    OPERATIONS ACT-(FREE) 
    0 RENT-VALUE ! ;

\ update rent value with profit at t if profit is greater 
: UPDATE-RENT-VALUE ( t -- ) 
    PROFIT@ RENT-VALUE @ MAX RENT-VALUE ! ;

\ update profit for a given time if n is greater 
: UPDATE-PROFIT ( n t -- )
    DUP PROFIT@ 
    ROT MAX 
    SWAP PROFIT! ;

\ update rent value at time t 
: DO-CASH ( t d p -- t d p )
    ROT DUP UPDATE-RENT-VALUE -ROT ;

\ update profit with p + rent value, at time t + d 
: DO-PLAN ( t d p -- ) 
    RENT-VALUE @ + 
    -ROT + UPDATE-PROFIT ;

\ update value at t then if price not null, plan the rent 
: PERFORM-OPERATION ( t d p -- ) 
    DO-CASH ?DUP IF DO-PLAN ELSE 2DROP THEN ;

\ we store an operation on a 63 bits key:
\ | 25 bits | 21 bits  | 17 bits |
\ |  time   | duration |  price  |

21 CONSTANT #DURATION
17 CONSTANT #PRICE

\ create binary mask of b bits 
: MASK ( b -- m ) 
    -1 SWAP LSHIFT INVERT ;

\ shift b bits in op then OR the value n in that op
: >>OPERATION ( n o b -- o' ) 
    LSHIFT OR ;

\ extract b bits from key into value n then shift key
: OPERATION>> ( k b -- n k' ) 
    2DUP MASK AND -ROT RSHIFT ; 

\ encode time duration and price in a key 
: >OPERATION ( t d p -- k )
    SWAP ROT #DURATION >>OPERATION #PRICE >>OPERATION ;

\ extract time, duration and price from a key
: OPERATION> ( k -- t d p )
    #PRICE OPERATION>> #DURATION OPERATION>> SWAP ROT ;

: OPERATIONS-! ( t d p  -- )
    >OPERATION NIL SWAP
    OPERATIONS ACT-INSERT ;

\ transform an order into a cash operation 
: CASH-TYPE ( t d -- t d p ) + 0 0 ;

\ this definition for symmetry ) 
: RENT-TYPE ( t d p -- t d p ) ;

\ store the cash and rent operations for an order ) 
: ADD-ORDER ( t d p -- ) 
    -ROT 2DUP CASH-TYPE OPERATIONS-!
     ROT      RENT-TYPE OPERATIONS-! ;

\ execute operation as retrieved in the tree ) 
: EXECUTE-OPERATION ( n k -- ) 
    OPERATION> PERFORM-OPERATION DROP ;

\ given operations stored after adding orders, compute rent value 
: CALC-RENT-VALUE ( -- ) 
    ['] EXECUTE-OPERATION OPERATIONS ACT-EXECUTE ;


