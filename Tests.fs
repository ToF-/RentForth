\ Tests.fs

REQUIRE Rent.fs
REQUIRE ffl/tst.fs

: TESTS
    ." after INIT PROFIT should be zero" CR
    4807 PROFIT !
    INIT
    T{ PROFIT @ 0 ?S }T 

    ." PLAN can store values for a given time" CR
    T{ 42 PLAN@ 0 ?S }T

    T{ 4807 42 PLAN!
       42 PLAN@ 4807 ?S }T

    ." UPDATE-PROFIT store value in profit if greater" CR
    T{  INIT
        100 UPDATE-PROFIT  PROFIT @ 100 ?S
         50 UPDATE-PROFIT  PROFIT @ 100 ?S 
        500 UPDATE-PROFIT  PROFIT @ 500 ?S }T

    ." UPDATE-PLAN store value in plan if greater" CR
    T{  INIT
        100 42 UPDATE-PLAN  42 PLAN@ 100 ?S
         50 42 UPDATE-PLAN  42 PLAN@ 100 ?S 
        500 42 UPDATE-PLAN  42 PLAN@ 500 ?S }T

    ." CASH update profit with plan at a given time" CR
    T{  INIT 
        500 42 PLAN!  PROFIT @   0 ?S
            42 CASH   PROFIT @ 500 ?S 
        100 53 PLAN! 
            53 CASH   PROFIT @ 500 ?S }T   

    ." RENT update plan at time+duration with profit+price" CR
    T{  INIT
        0 5 100 RENT
        3 7 140 RENT
        5       CASH
        5 9  80 RENT
        6 9  70 RENT
        10      CASH
        14      CASH
        15      CASH
        PROFIT @ 180 ?S }T

    ." TIME-DURATION>KEY encode time and duration" CR
    T{  5 0 TIME-DURATION>KEY 
        5 9 TIME-DURATION>KEY < TRUE ?S }T

    ." KEY>TIME-DURATION decode key to time and duration" CR
    T{  42 89 TIME-DURATION>KEY
        KEY>TIME-DURATION  89 ?S 42 ?S }T

    ." {CASH} store a cash action" CR
    T{  INIT-ACTIONS
        5 {CASH}
        5 0 TIME-DURATION>KEY ACTIONS ACT-GET
        TRUE ?S 0 ?S }T

    ." {RENT} store a rent action with duration and price" CR
    T{  INIT-ACTIONS
        5 9 40 {RENT}
        5 9 80 {RENT}
        5 9 70 {RENT}
        5 9 TIME-DURATION>KEY ACTIONS ACT-GET
        TRUE ?S 80 ?S }T

    ." ADD-ORDER store cash and rent actions" CR
    T{  INIT-ACTIONS
        0 5 100 ADD-ORDER 
        5 0 TIME-DURATION>KEY ACTIONS ACT-GET
        TRUE ?S 0 ?S
        0 5 TIME-DURATION>KEY ACTIONS ACT-GET
        TRUE ?S 100 ?S }T

    ." COMPUTE-PROFIT compute best profit best value" CR
    T{  INIT-ACTIONS
        0 5 100 ADD-ORDER
        3 7 140 ADD-ORDER
        5 9  80 ADD-ORDER
        6 9  70 ADD-ORDER
        COMPUTE-PROFIT
        PROFIT @ 180 ?S }T
;
TESTS
BYE
