\ Tests.fs
INCLUDE ffl/tst.fs
INCLUDE Rent.fs

: TEST-VALUE-TABLE
    42 4807 SET-VALUE
    T{ 4807 GET-VALUE -1 ?S  42 ?S }T
    43 4807 SET-VALUE
    T{ 4807 GET-VALUE -1 ?S  43 ?S }T
    41 4807 SET-VALUE
    T{ 4807 GET-VALUE -1 ?S  43 ?S }T
;


: TESTS
    TEST-VALUE-TABLE
;

TESTS
." success"
BYE
