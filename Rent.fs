\ Rent.fs
INCLUDE ffl/hct.fs

20000 CONSTANT MAX-VALUES
MAX-VALUES HCT-CREATE VALUE-TABLE 

: TO-STRING ( n -- adr,cnt )
    0 <# #S #> ;

: SET-VALUE ( n,k -- )
    DUP TO-STRING VALUE-TABLE HCT-GET 
    IF ROT MAX SWAP THEN
    TO-STRING VALUE-TABLE HCT-INSERT ;
             
         

: GET-VALUE ( k -- n,f | false )
    TO-STRING VALUE-TABLE HCT-GET ;

    
