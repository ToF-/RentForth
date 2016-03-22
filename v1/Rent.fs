\ Rent.fs

1 21 LSHIFT 1- CONSTANT MASK

: RENT-ORDER ( st,du,pr -- order )
    ROT 21 LSHIFT 
    ROT OR 21 LSHIFT
    SWAP OR ;

: ORDER>SDP ( order -- st,du,pr )
    DUP MASK AND >R
    21 RSHIFT DUP MASK AND >R
    21 RSHIFT R> R> ;

: ORDER>BID ( order -- bid )
    MASK AND ;

: ORDER>DURATION ( order -- dur )
    21 RSHIFT MASK AND ;
 
: ORDER>START ( order -- start )
    42 RSHIFT MASK AND ;

    

    
    
