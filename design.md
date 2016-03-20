
processing these orders: (start X duration X money)
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
0  5  100
3  7  140
5  9   70
6  9   80
10 4   10 
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

requires a mapping of time X (time,money) for values :
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
0  : -1 
3  : -1
5  : -1
6  : -1
10 : -1
14 : -1
15 : -1
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

then iterate on the order list keeping track of the 
max of money made, and calculate the value for each time
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
M <- 0 V <- 0
0 5 100 : t[0] <- max t[0] M ; t[5]  <- max t[5]  t[0] + 100 ; V <- max V t[5] ; M <- max M t[0] ( M=0  t[5]=100 V=100 )
3 7 140 : t[3] <- max t[3] M ; t[10] <- max t[10] t[3] + 140 ; V <- max V t[10] ; M <- max M t[3] ( M=0  t[10]=140 V=140 )
5 9  70 : t[5] <- max t[5] M ; t[14] <- max t[14] t[5] +  70 ; V <- max V t[14] ; M <- max M t[5] ( M=100 t[14]=170 V=170 )
6 9  80 : t[6] <- max t[6] M ; t[15] <- max t[15] t[6] +  80 ; V <- max V t[15] ; M <- max M t[6] ( M=100 t[15]=180 V=180 )
10 4 10 : t[10] <- max t[10] M ; t[14] <- max t[14] t[10] + 10 ; V <- max V t[14] M <- max M t[10] ( M=170 t[14]=170 V=180 )

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

 

 









