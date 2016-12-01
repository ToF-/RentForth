\ Tests.fs

REQUIRE Rent.fs
REQUIRE ffl/tst.fs

: TESTS
    ." after initialize, profit should be zero" CR
    INITIALIZE
    T{ RENT-VALUE @ 0 ?S }T 

    ." rent action followed by update profit should increase profit" CR
    INITIALIZE
    3 7 140 PERFORM-OPERATION
    10 0 0  PERFORM-OPERATION
    T{ RENT-VALUE @ 140 ?S }T 

    ." rent actions correctly sequenced should determine max profit" CR
    INITIALIZE
    0 5 100 PERFORM-OPERATION
    3 7 140 PERFORM-OPERATION
    5 9  80 PERFORM-OPERATION
    6 9  70 PERFORM-OPERATION
    10 0  0 PERFORM-OPERATION
    14 0  0 PERFORM-OPERATION
    15 0  0 PERFORM-OPERATION
    T{ RENT-VALUE @ 180 ?S }T

    ." rent action should be composed and decomposed" CR
    3 7 140 >OPERATION OPERATION> 
    T{ 140 ?S 7 ?S 3 ?S }T
    10 0 0  >OPERATION OPERATION>
    T{ 0 ?S 0 ?S 10 ?S }T 

    ." update action should be lower than rent on same time" CR
    3  7  0 >OPERATION 
    10 1  1 >OPERATION
    T{ < -1 ?S }T

    ." after initialize, actions should be zero" CR
    INITIALIZE
    T{ OPERATIONS ACT-LENGTH@ 0 ?S }T 

    ." adding order generates 2 actions" CR
    INITIALIZE
    3 7 140 ADD-ORDER
    T{ OPERATIONS ACT-LENGTH@ 2 ?S }T

    ." calc-profit should calculate profit made with orders" CR
    INITIALIZE
    6 9  70 ADD-ORDER
    5 9  80 ADD-ORDER
    3 7 140 ADD-ORDER
    0 5 100 ADD-ORDER
    CALC-RENT-VALUE
    T{ RENT-VALUE @ 180 ?S }T

;
TESTS
BYE
