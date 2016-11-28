Rent your Airplane and Make Money
=================================
1. A Request
------------

Here is what we received:

> "ABEAS Corp." is a very small company that owns a single airplane. The customers
of ABEAS Corp are large airline companies which rent the airplane to accommodate
occasional overcapacity.
>
> Customers send renting orders that consist of a time interval and a price that
the customer is ready to pay for renting the airplane during the given time
period. Orders of all the customers are known in advance. Of course, not all
orders can be accommodated and some orders have to be declined. Eugene LAWLER,
the Chief Scientific Officer of ABEAS Corp would like to maximize the profit of
the company.
>
> You are requested to compute an optimal solution.

There's also a small example:

> Consider for instance the case where the company has 4 orders:
>
> 1.  Order 1 (start time 0, duration 5, price 100)
> 2.  Order 2 (start time 3, duration 7, price 140)
> 3.  Order 3 (start time 5, duration 9, price 80)
> 4.  Order 4 (start time 6, duration 9, price 70)
> 
> The optimal solution consists in declining Order 2 and 3 and the gain is 10+8 =
180. Note that the solution made of Order 1 and 3 is feasible (the airplane is
rented with no interruption from time 0 to time 14) but non-optimal.

The constraints are as follow:

- Number of orders n: n ≤ 10000
- for each order:
    - start time: 0 ≤ t \< 1000000
    - duration: 0 \< d \< 1000000
    - price: 0 \< p \< 100000


2. The Formula
--------------

We can determine the profit of a list of N orders using this formula

S = maximum { Vi,Vi+1,..Vn }
 
Vi = p(i) + maximum { Vj | j=1..n, t(j) >= t(i)+d(i) }

where t(i),d(i) and p(i) = the start time, duration and price for Order(i)

Applied to the example above:

    S = maximum { V(1),V(2),V(3),V(4) }

    V(1) = 100 + maximum { V(3),V(4) }
         = 100 + maximum { 70 + maximum { }, 80 + maximum { } }
         = 180
    V(2) = 140  + maximum { }
    V(3) = 80  + maximum { }
    V(4) = 70  + maximum { }

Infortunately this formula is not practical as the computation would require N! comparisons.

Another way to compute the solution is to define P(t), the profit value at time t, and consider that

    ∀ t,t'  t'>t ⇒  P(t') ≥ P(t) 
    ∀ t,d>0,p>0 | Order(t,d,p) ⇒ P(t+d) ≥ P(t)+p  
    
Applied to our example:

    Order(0,5,100) ⇒ P(5)  ≥ P(0)+100 ⇒ P(5) ≥ 100
    Order(3,7,140) ⇒ P(10) ≥ P(3)+140 ⇒ P(10) ≥ 140
    Order(5,9, 80) ⇒ P(14) ≥ P(5)+80 ⇒ P(14) ≥ 180
    Order(6,9, 70) ⇒ P(15) ≥ P(6)+70 ⇒ P(15) ≥ 170
                     P(15) ≥ P(14) ⇒ P(15) ≥ 180 

Rather than searching the order list for the maximum value recursively, we can process each calculation at the right time, while keeping track of the profit made so far. Each order(t,d,p) involve two Operations

- at time t, plan the renting the airplane, thus writing a profit increase by p at time t+d
- at time t+d, update V the profit value made so far


    Order(t,d,p) ⇒ Operation(t, P[t+d] := max(V, P[t]+p)), Operation(t+d, V := max(V, P[t+d]))


Given a sequence of Actions that is ordered on t, if we execute each Operation:

1. update the profit value made so far
2. plan the ulterior profit made by rent at price + profit 

then we end up with the maximum profit value made with all the orders.

3. Mapping time to money
------------------------

We want to solve this problem in Forth, using gforth. In addition to the standard data structures (a stack of 64 bits words and a dictionnary for our variables) we need to be able to map a time value to a money value. If time values were not so large, Using standard memory with `ALLOT` would be quite possible:

    2000 CONSTANT MAX-TIME
    CREATE PROFIT MAX-TIME CELLS ALLOT
    PROFIT MAX-TIME CELLS ERASE

    : PROFIT! ( n t --    store profit n at time t )
        CELLS PROFIT + ! ;

    : PROFIT@ ( t -- n    retrieve profit a time t )
        CELLS PROFIT + @ ;

    4807 500 PROFIT!
    500 PROFIT@ .
    4807 ok

But time values can be as large as 2000000, so that solution won't work:

	2000000 CONSTANT MAX-TIME  ok
	CREATE PROFIT MAX-TIME CELLS ALLOT
	:2: Dictionary overflow
	CREATE PROFIT MAX-TIME CELLS >>>ALLOT<<<
	Backtrace:
	$10568E8E0 throw

Besides, using such large dictionary space for only 10000 entries at last would be wasteful.

Enters `act` a module from the Forth Foundation Library. This module provides us with the ability to store key/values in AVL trees.

	REQUIRE ffl/act.fs

	ACT-CREATE PROFITS

	: PROFIT! ( n t --    store profit n at time t )
		PROFITS ACT-INSERT ;

	: PROFIT@ ( t -- n    retrieve profit a time t or 0 if not found )
		PROFITS ACT-GET 0= IF 0 THEN ;

	4807 500000 PROFIT! ⏎
	ok
	500000 PROFIT@ . ⏎
	4807  ok
	234 PROFIT@ . ⏎ 
	0  ok 


