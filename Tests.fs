\ Tests.fs
REQUIRE ffl/tst.fs
REQUIRE Rent.fs

: TEST-ACTIONS
    INITIALIZE
    100 5  PLAN-RENT 
    140 10 PLAN-RENT 
        5  UPDATE-PROFIT 
     80 14 PLAN-RENT 
     70 15 PLAN-RENT 
        10 UPDATE-PROFIT 
        14 UPDATE-PROFIT 
        15 UPDATE-PROFIT 
    T{ PROFIT @ 180 ?S }T ;


TEST-ACTIONS 
PLAN 5  + C@ .
PLAN 10 + C@ .
PLAN 14 + C@ .
PLAN 15 + C@ .


BYE



