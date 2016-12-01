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

    ." CASH update PROFIT with plan at a given time" CR
    T{  INIT 
        500 42 PLAN!  PROFIT @   0 ?S
            42 CASH   PROFIT @ 500 ?S 
        100 53 PLAN! 
            53 CASH   PROFIT @ 500 ?S }T   

    ." RENT update PLAN at time+duration with PROFIT+price" CR
    T{  INIT
        100 42 PLAN!
        42 10 100 RENT   
        52 PLAN@  200 ?S }T
    
      
;
TESTS
BYE
