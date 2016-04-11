\ Tests.fs

REQUIRE Rent.fs
REQUIRE ffl/tst.fs

: TESTS
    ." after initialize, profit should be zero" CR
    INITIALIZE
    T{ PROFIT @ 0 ?S }T 

    ." rent action followed by update profit should increase profit" CR
    INITIALIZE
    3 7 140 RENT-AIRPLANE
    10      UPDATE-PROFIT
    T{ PROFIT @ 140 ?S }T 

    ." rent actions correctly sequenced should determine max profit" CR
    INITIALIZE
    0 5 100 RENT-AIRPLANE
    3 7 140 RENT-AIRPLANE
    5       UPDATE-PROFIT
    5 9  80 RENT-AIRPLANE
    6 9  70 RENT-AIRPLANE
    10      UPDATE-PROFIT
    14      UPDATE-PROFIT
    15      UPDATE-PROFIT
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

    ." after initialize, actions should be zero" CR
    INITIALIZE
    T{ ACTIONS ACT-LENGTH@ 0 ?S }T 

    ." adding order generates actions" CR
    INITIALIZE
    3 7 140 ADD-ORDER
    T{ ACTIONS ACT-LENGTH@ 2 ?S }T

    ." calc-profit should calculate profit made with orders" CR
    INITIALIZE
    6 9  70 ADD-ORDER
    5 9  80 ADD-ORDER
    3 7 140 ADD-ORDER
    0 5 100 ADD-ORDER
    CALC-PROFIT
    T{ PROFIT @ 180 ?S }T

;
TESTS
BYE
