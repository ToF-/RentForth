
these orders:
0  5  100
3  7  140
5  9   70
6  9   80
10 3   50 

generate events:

5  t[5] = max t[0] + 100 t[5]
6  t[6] = max t[5] + 0 t [6]
10 t[10]= max t[3] + 140 t[10]
13 t[13]= max t[10]+ 50  t[13]
14 t[14]= max t[5] + 70  t[14]
15 t[15]= max t[6] + 80  t[15]

then processing events:

0 m <- 0
3 m <- 0
5 m <- 0
5 m <- 100
6 m <- 100
10 
14
15



