\ Tests.fs
INCLUDE ffl/tst.fs
INCLUDE Rent.fs

: TEST-VALUE-TIME-TABLE
    INIT-VALUES
    42 4807 UPDATE-VALUE
    T{ 4807 GET-VALUE -1 ?S  42 ?S }T
    43 4807 UPDATE-VALUE
    T{ 4807 GET-VALUE -1 ?S  43 ?S }T
    41 4807 UPDATE-VALUE
    T{ 4807 GET-VALUE -1 ?S  43 ?S }T
;

VARIABLE V

: TEST-UPDATE-MAX
    INIT-VALUES
    0 V !
    10 V UPDATE-MAX
    T{ V @ 10 ?S }T 
    5 V UPDATE-MAX
    T{ V @ 10 ?S }T 
;

: TEST-ADD-ORDER
    INIT-VALUES
    0 5 10 ADD-ORDER
    T{ MAX-VALUE @ 10 ?S }T
    3 7 14 ADD-ORDER
    T{ MAX-VALUE @ 14 ?S }T
    5 9 7 ADD-ORDER
    T{ MAX-VALUE @ 17 ?S }T
    6 9 8 ADD-ORDER
    T{ MAX-VALUE @ 18 ?S }T
    10 3 7 ADD-ORDER
    T{ MAX-VALUE @ 21 ?S }T
;

: TEST-UPDATE-VALUE-AT-START-TIME
    INIT-VALUES
    42 CURRENT-VALUE ! 
    0 UPDATE-VALUE-AT-START-TIME 
    T{ 0 GET-VALUE TRUE ?S 42 ?S }T ;    
    


: TESTS
    TEST-VALUE-TIME-TABLE
    TEST-UPDATE-MAX
    TEST-UPDATE-VALUE-AT-START-TIME
    TEST-ADD-ORDER
;
PAGE
TESTS
BYE
