\ Rent.fs

VARIABLE PROFIT 
CREATE PLAN 100 CELLS ALLOT

: INITIALIZE
    PLAN 100 CELLS ERASE
    0 PROFIT ! ;

: PLAN# ( time -- addr )
    CELLS PLAN + ;

: PLAN@ ( time -- n  )
    PLAN# @ PROFIT @ MAX ;
 
: PLAN! ( n time -- )
    PLAN# DUP @ ROT MAX SWAP ! ;
 
: CASH ( time -- )
    PLAN@ PROFIT ! ;
      
: RENT ( end price -- )
    PROFIT @ + SWAP PLAN! ;
    
    

