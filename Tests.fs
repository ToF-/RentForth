\ Tests.fs

REQUIRE Rent.fs
REQUIRE ffl/tst.fs

: TESTS
    ." after initialize, profit should be zero" CR
    INITIALIZE
    T{ PROFIT @ 0 ?S }T 

    ." rent action followed by update profit should increase profit" CR
    INITIALIZE
    3 7 140 RENT
    10 UPDATE-PROFIT
    T{ PROFIT @ 140 ?S }T 

    ." rent actions correctly sequenced should determine max profit" CR
    INITIALIZE
    0 5 100 RENT
    3 7 140 RENT
    5 UPDATE-PROFIT
    5 9  80 RENT
    6 9  70 RENT
    10 UPDATE-PROFIT
    14 UPDATE-PROFIT
    15 UPDATE-PROFIT
    T{ PROFIT @ 180 ?S }T

    ." rent action should be composed and decomposed" CR
    3 7 140 ACTION>KEY KEY>ACTION 
    T{ 140 ?S 7 ?S 3 ?S }T
    3 7 0   ACTION>KEY KEY>ACTION
    T{ 0 ?S 0 ?S 10 ?S }T 

    ." update action should be lower than rent on same time" CR
    3  7  0 ACTION>KEY 
    10 1  1 ACTION>KEY
    T{ < -1 ?S }T
;
TESTS
BYE
