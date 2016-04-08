\ Rent.fs

VARIABLE RENT-VALUE 
CREATE PLAN 100 CELLS ALLOT

: INITIALIZE
    PLAN 100 CELLS ERASE
    0 RENT-VALUE ! ;

: PLAN# ( time -- addr )
    CELLS PLAN + ;

: PLAN@ ( time -- n  )
    PLAN# @ RENT-VALUE @ MAX ;
 
: PLAN! ( n time -- )
    PLAN# DUP @ ROT MAX SWAP ! ;
 
: CASH ( time -- )
    PLAN@ RENT-VALUE @ MAX RENT-VALUE ! ;
      
: RENT ( end price -- )
    RENT-VALUE @ + SWAP PLAN! ;
    
    

