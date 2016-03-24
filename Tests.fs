\ Tests.fs gforth Tests.fs

INCLUDE ffl/tst.fs
INCLUDE Rent.fs

VARIABLE V

: TEST-MAX!
    0  V !  42 V MAX!  T{ V @ 42 ?S }T
    45 V MAX!  T{ V @ 45 ?S }T 
    33 V MAX!  T{ V @ 45 ?S }T
;
: TEST-VALUE#@
    INITIALIZE
    4807 VALUE#@ T{ 0 ?S }T 
    42 4807 VALUE#-MAX! 4807 VALUE#@ T{ 42 ?S }T 
    33 4807 VALUE#-MAX! 4807 VALUE#@ T{ 42 ?S }T 
    54 4810 VALUE#-MAX! T{ 4810 VALUE#@ 54 ?S 4807 VALUE#@ 42 ?S }T
;

: TESTS
    TEST-MAX!
    TEST-VALUE#@
;

TESTS BYE
    
