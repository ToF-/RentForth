\ Tests.fs
INCLUDE ffl/tst.fs
INCLUDE Rent.fs

: TEST-VALUE-TIME-TABLE
    INITIALIZE
    42 4807 UPDATE-VALUE
    T{ 4807 GET-HCT-VALUE -1 ?S  42 ?S }T
    43 4807 UPDATE-VALUE
    T{ 4807 GET-HCT-VALUE -1 ?S  43 ?S }T
    41 4807 UPDATE-VALUE
    T{ 4807 GET-HCT-VALUE -1 ?S  43 ?S }T
;

VARIABLE V

: TEST-UPDATE-MAX
    INITIALIZE
    0 V !
    10 V UPDATE-MAX
    T{ V @ 10 ?S }T 
    5 V UPDATE-MAX
    T{ V @ 10 ?S }T 
;

: TEST-ADD-ORDER
    INITIALIZE
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
    INITIALIZE
    42 CUR-VALUE ! 
    0 UPDATE-VALUE-AT-START-TIME 
    T{ 0 GET-HCT-VALUE TRUE ?S 42 ?S }T ;    
    
: TEST-TWO-COLLAPSING-ORDERS
    INITIALIZE
    0 5 14 ADD-ORDER
    3 7 10 ADD-ORDER
    T{ MAX-VALUE @ 14 ?S }T ;

: TEST-TWO-FOLLOWING-ORDERS
    INITIALIZE
    0 5 10 ADD-ORDER
    5 9  7 ADD-ORDER
    T{ MAX-VALUE @ 17 ?S }T ;

: TESTS
    TEST-VALUE-TIME-TABLE
    TEST-UPDATE-MAX
    TEST-UPDATE-VALUE-AT-START-TIME
    TEST-ADD-ORDER
    TEST-TWO-COLLAPSING-ORDERS
    TEST-TWO-FOLLOWING-ORDERS
;
PAGE
TESTS
BYE
