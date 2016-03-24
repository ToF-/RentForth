
processing these orders: (start time X duration X bid)
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
0  5  100
3  7  140
5  9   70
6  9   80
10 4   10 
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

requires T a mapping of time X (time,money) for times : 0 3 5 6 10 14 15 with default value = 0

iterate on the order list, keeping track of 3 values:

- V : maximum value made at start time of current order
- T[start+end] : V + bid
- M : maximum value possible at the end for current order

V is either the value at start time, or a better value made before start time:
V <- max(t[start],V)

value at end of order is V + bid or a better value already made at that time
T[start+duration] <- max(t[start+duration],V + bid)

M is either the new value at end of order, or a better value at the end of another order previously computed
M <- max(t[start+duration],M)

required words:

- access to hash table with default value = 0
- creating a string key for long integers using base 64 conversion
- updating the hash table when new value > existing value
- "maxing" a variable with a value : X <- max X v


 

 









