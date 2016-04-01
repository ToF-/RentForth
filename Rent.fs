\ Rent.fs
VARIABLE EVENT-ID

0 CONSTANT CASH
1 CONSTANT RENT
1  CONSTANT KIND%
15 CONSTANT KEY%
24 CONSTANT TIME%
: INIT-EVENT-ID 0 EVENT-ID ! ;
: NEXT-ID ( -- id ) EVENT-ID DUP @ 1 ROT +! ;
: <<KIND! ( kind -- n )        KIND% LSHIFT OR ;
: <<KEY!  ( n id -- key )       KEY% LSHIFT OR ;
: <<TIME! ( bid time -- data ) TIME% LSHIFT OR ;
: EVENT-KEY ( kind time -- key ) <<KIND! NEXT-ID SWAP <<KEY! ;
: EVENT-DATA ( start duration bid kind -- data )
    IF -ROT + <<TIME! ELSE 0 THEN ;
: RENT-EVENT ( start duration bid -- data key )
    ROT DUP >R -ROT RENT EVENT-DATA RENT R> EVENT-KEY ;
: CASH-EVENT ( start duration bid -- data key )
    CASH EVENT-DATA CASH ROT EVENT-KEY ;
