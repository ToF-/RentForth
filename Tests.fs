\ Tests.fs  gforth Tests.fs

INCLUDE Rent.fs

: EQUALS ( n1,n2 -- f with message if false )
    2DUP <> IF ." EXPECTED:" . ." BUT WAS:" . FALSE
    ELSE 2DROP TRUE THEN ;
    
: TEST-EQUALS
    ASSERT( 2 2 + 4 EQUALS ) ;

: TESTS
    TEST-EQUALS
;

TESTS
." Success"
.S
BYE
