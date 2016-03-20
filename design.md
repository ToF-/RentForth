
processing these orders: (start X duration X money)
    0  5  100
    3  7  140
    5  9   70
    6  9   80
    10 4   50 

requires a mapping of time X (time,money) for values :
    0  :- -1 0
    3  :-  0 0
    5  :-  0 100
    6  :-  5 0
    10 :-  3 140
    14 :-  5 70
    14 :- 10 50
    15 :-  6 80

then traverse the map keeping track of the max of money made, and calculate the value for each time
     0   0 :- -1 0
     0   3 :- max( M, t[0] + 0) = 0
     0   5 :- max( M, t[0] + 100) = 100
    100  6 :- max( M, t[5] + 0) = 100
    100 10 :- max( M, t[3] + 140) = 140
    140 14 :- max( M, t[5] + 70) = 170
    170 14 :- max( M, t[10] + 50) = 190
    190 15 :- max( M, t[6] + 80) = 190
 
 









