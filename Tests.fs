\ Tests.fs gforth Tests.fs

INCLUDE ffl/tst.fs
INCLUDE Rent.fs

VARIABLE V

: TEST-MAX!
    0  V !  42 V MAX!  T{ V @ 42 ?S }T
    45 V MAX!  T{ V @ 45 ?S }T 
    33 V MAX!  T{ V @ 45 ?S }T
;
: TEST-S>KEY
    65 S>KEY S" 65" COMPARE T{ 0 ?S }T ;

: TEST-VALUE#@
    INITIALIZE
    4807 VALUE#@ T{ 0 ?S }T 
    42 4807 VALUE#-MAX! 4807 VALUE#@ T{ 42 ?S }T 
    33 4807 VALUE#-MAX! 4807 VALUE#@ T{ 42 ?S }T 
    54 4810 VALUE#-MAX! T{ 4810 VALUE#@ 54 ?S 4807 VALUE#@ 42 ?S }T
;

: TEST-ADD-ORDER
    INITIALIZE
    0 5 10 ADD-ORDER T{ MAX-VALUE @ 10 ?S }T 
    3 7 14 ADD-ORDER T{ MAX-VALUE @ 14 ?S }T 
    5 9  7 ADD-ORDER T{ MAX-VALUE @ 17 ?S }T 
    6 9  8 ADD-ORDER T{ MAX-VALUE @ 18 ?S }T 
    10 4 7 ADD-ORDER T{ MAX-VALUE @ 21 ?S }T 
; 
: TEST-HCT
    INITIALIZE
    20000 0 DO I DUP VALUE#-MAX! LOOP
    20000 0 DO T{ I VALUE#@ I ?S }T LOOP ;

: TESTS
    TEST-MAX!
    TEST-S>KEY
    TEST-VALUE#@
    TEST-HCT
    TEST-ADD-ORDER
;

TESTS BYE
    
