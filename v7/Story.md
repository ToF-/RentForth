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

V = maximum { Pi,Pi+1,..Pn }
 
Pi = p(i) + maximum { Pj | j=1..n, t(j) >= t(i)+d(i) }

where t(i),d(i) and p(i) = the start time, duration and price for an order(i)

Applied to the example above:

    V = maximum { P(1),P(2),P(3),P(4) }

    P(1) = 100 + maximum { P(3),P(4) }
         = 100 + maximum { 80 + maximum { }, 70 + maximum { } }
         = 180
    P(2) = 140  + maximum { }
    P(3) = 80  + maximum { }
    P(4) = 70  + maximum { }
    V    = 180

Infortunately this formula is not practical as the computation would require N! comparisons.

Another way to compute the solution is to define P(t), the profit value at time t, and consider that:

    P(0) ≥ 0
    ∀ t,t'  t'>t ⇒  P(t') ≥ P(t) 
    ∀ t,d>0,p>0 | Order(t,d,p) ⇒ P(t+d) ≥ P(t)+p  
    V ≥ P(t) | t = maximum { s+d | Order(s,d,p) }
    
Let's apply those rules to the case given as an example:

    Order(0,5,100) ⇒ P(5)  ≥ P(0)+100
    P(0) ≥ 0       ⇒ P(5)  ≥ 100
    Order(3,7,140) ⇒ P(10) ≥ P(3)+140
    P(3) ≥ P(0)    ⇒ P(10) ≥ 140
    Order(5,9, 80) ⇒ P(14) ≥ P(5)+80
    P(5) ≥ 100     ⇒ P(14) ≥ 180
    Order(6,9, 70) ⇒ P(15) ≥ P(6)+70
    P(6) ≥ P(5)    ⇒ P(15) ≥ 170
    P(15) ≥ P(14)  ⇒ P(15) ≥ 180
    V ≥ P(15)      ⇒ S ≥ 180

This suggest the following algorithm for solving our problem:

    using a planner with a cell for each time value,
    for each order starting at s with duration d for price p,
        make a note that at time s,
            we will need to udpate the maximum value V with the profit value for that time if it is greater than V
            and then plan a profit at time s+d to be at least V + p
            (this is an UPDATE_AND_RENT operation)
 
        make a note that at time s+d, 
            we will need to udpate the maximum value V with the profit value for that time if it is greater than V
            (this an UPDATE operation)

    then running through the planner in chronological order:
        perform the operation is there is one


Let's try this algoritm with our example case. First, we check our order list:

    order at 0 duration 5, price 100:
        note in cell 0 : UPDATE_AND_RENT at time 5, price V+100
        note in cell 5 : UPDATE
    order at 3 duration 7, price 140:
        note in cell 3  : UPDATE_AND_RENT at time 10, price V+140
        note in cell 10 : UPDATE
    order at 5 duration 9, price 80:
        note in cell 5  : UPDATE_AND_RENT at time 14, price V+80
        note in cell 14 : UPDATE
    order at 6 duration 9, price 70:
        note in cell 6  : UPDATE_AND_RENT at time 15, price V+70
        note in cell 15 : UPDATE

Then we run through the planner, starting with V = 0:       

    0 UPDATE_AND_RENT 5  100 : V  ← max(V,P(0)) = max(0,0) = 0
                               P(5)  ← max(P(5),V+100) = max(0, 100) = 100
    3 UPDATE_AND_RENT 10 140 : V  ← max(V,P(3)) = max(0,0) = 0
                               P(10) ← max(P(10),V+140) = max(0, 140) = 140
    5 UPDATE                 : V ← max(V, P(5)) = max(0,100) = 100

    5 UPDATE_AND_RENT 14 80  : V ← max(V,P(5)) = max(100,100) = 100
                               P(14) ← max(P(14),V+80) = max(0,180) = 180 
    6 UPDATE_AND_RENT 15 70  : V ← max(V,P(6)) = max(100,0) = 100
                               P(15) ← max(P(15),V+70) = max(0,170) = 170 
    10 UPDATE                : V ← max(V, P(10)) = max (100, 140) = 140 
    14 UPDATE                : V ← max(V, P(14)) = max (140, 180) = 180 
    15 UPDATE                : V ← max(V, P(15)) = max (180, 170) = 180 

And V is now the maximum profit value we can draw from the orders. 

3. A Divide and Conquer Approach

We want to solve this problem in Forth, using gforth. Let's first decompose our rather big problem into smaller ones:

1. defining and updating a global money value V 
2. retrieving and updating values in a table P that maps time values to money values 
3. for each order entered by the user, memorizing 2 actions in a list
4. sorting the list of actions by time then category of action (update, or update_and_rent)
5. executing all the actions in the sorted list

Defining a money value is very easy:

	VARIABLE RENT-VALUE
	0 RENT-VALUE !

Let's start with a simple proof of concept: we will pretend for a moment that start time can only be comprised betmeen 0 and 100 as well as duration time. This allow for our profit planner to reside in the dictionnary:

    CREATE PROFIT 200 CELLS ALLOT
    PROFIT 200 CELLS ERASE   

    : PROFIT@ ( t -- m   finds profit value at time t or 0 )
        CELLS PROFIT + @ ;
         
    : PROFIT! ( m t --   stores profit value at time t )
        CELLS PROFIT + ! ;

Let's try our table:

	42 PROFIT@ .    ⏎  0 ok
	4807 42 PROFIT! ⏎  ok
	42 PROFIT@ .    ⏎  4807 ok

Now we need definition for actions:

	: UPDATE-VALUE ( t --   update value with profit at time t if greater )
		PROFIT@ RENT-VALUE @ MAX
		RENT-VALUE ! ;

    : UPDATE-PROFIT ( p t -- update profit at time t with p if p is greater )
        DUP PROFIT@ 
        ROT MAX 
        SWAP PROFIT! ;
         
    : UPDATE-AND-RENT ( s d p  -- update profit table at s+d for price p )
        ROT DUP UPDATE-VALUE
        SWAP RENT-VALUE @ +
        -ROT + UPDATE-PROFIT ;
	
Let's ignore for now the problem of storing action events and retrieving them in order, and pretend we can just execute these actions in the right order:

    0 5 100 UPDATE-AND-RENT
    3 7 140 UPDATE-AND-RENT
    5       UPDATE-VALUE
    5 9 80  UPDATE-AND-RENT
    6 9 70  UPDATE-AND-RENT
    10      UPDATE-VALUE
    14      UPDATE-VALUE
    15      UPDATE-VALUE
    RENT-VALUE ? ⏎  180 ok

4. Mapping time to money
------------------------

Of course, the specs for the requested program mention that time values can be as large as 2000000, so our solution of storing the PROFIT table in the dictionary won't work:

	2000000 CONSTANT MAX-TIME  ok
	CREATE PROFIT MAX-TIME CELLS ALLOT
	:2: Dictionary overflow
	CREATE PROFIT MAX-TIME CELLS >>>ALLOT<<<
	Backtrace:
	$10568E8E0 throw

Besides, using such large dictionary space for only 10000 time point entries at last would be wasteful.

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


