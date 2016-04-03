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
    T{ 12 PLAN@ 17 ?S }T ;

    
TEST-PLAN
BYE

