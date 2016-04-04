\ Tests.fs
REQUIRE ffl/tst.fs
REQUIRE Rent.fs

: TEST-PLAN
    INITIALIZE
    5 10 RENT
    T{ 5 PLAN@ 10 ?S }T 
    5 CASH
    T{ RENT-VALUE @ 10 ?S }T
    12 7 RENT
    T{ 12 PLAN@ 17 ?S }T
    12 5 RENT
    T{ 12 PLAN@ 17 ?S }T 
    12 CASH
    T{ RENT-VALUE @ 17 ?S }T ; 

: TEST-RUN-EVENT 
    INITIALIZE
    5     10 RUN-EVENT
    10    14 RUN-EVENT
    5  CASH% RUN-EVENT
    14     7 RUN-EVENT
    15     8 RUN-EVENT
    10 CASH% RUN-EVENT
    14 CASH% RUN-EVENT
    15 CASH% RUN-EVENT
    T{ RENT-VALUE @ 18 ?S }T ;

: TEST-EVENT-KEY
    INITIALIZE
    12 RENT% EVENT-KEY [ 2 BASE ! ]
    T{ 11001000000000000000 ?S }T [ DECIMAL ] 
    14 RENT% EVENT-KEY [ 2 BASE ! ]
    T{ 11101000000000000001 ?S }T [ DECIMAL ] 
    12 CASH% EVENT-KEY [ 2 BASE ! ]
    T{ 11000000000000000010 ?S }T [ DECIMAL ] ;

: TEST-EVENT-DATA
    17 10 RENT-DATA [ 2 BASE ! ]
    T{ 10001000000000000000000001010 ?S }T [ DECIMAL ] ;  

: TEST-ADD-ORDER
    INITIALIZE
    0 5 10 ADD-ORDER
    T{ EVENTS @ ACT-LENGTH@ 2 ?S }T ;

\ : TEST-COMPUTE-VALUE
\     INITIALIZE
\     0 5 10 ADD-ORDER
\     3 7 14 ADD-ORDER
\     5 9  7 ADD-ORDER
\     6 9  8 ADD-ORDER
\     T{ COMPUTE-VALUE 18 ?S }T ;
    
: TEST-GET-EVENT
    [ 2 BASE ! ]
    11001000000000000000
    10001000000000000000000001010
    GET-EVENT [ DECIMAL ]
    T{ 17 10 RENT% ?S ?S ?S }T ;

TEST-PLAN
TEST-RUN-EVENT
TEST-EVENT-KEY
TEST-EVENT-DATA
TEST-ADD-ORDER
DBG TEST-GET-EVENT
\ TEST-COMPUTE-VALUE

BYE

