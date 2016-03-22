\ TestRent.fs

INCLUDE Rent.fs

: TEST-ORDER
    2 3 10  RENT-ORDER  
    ASSERT( DUP ORDER>BID 10 EQUALS ) 
    ASSERT( DUP ORDER>DURATION 3 EQUALS ) 
    ASSERT(  ORDER>START 2 EQUALS ) 

    999999 999899 99999 RENT-ORDER

    ASSERT( DUP ORDER>BID 99999 EQUALS ) 
    ASSERT( DUP ORDER>DURATION 999899 EQUALS ) 
    ASSERT( ORDER>START 999999 EQUALS ) 
;

: TESTS-RENT
    TEST-ORDER
;