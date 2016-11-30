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

    Planning the orders
    
    using a planner with a page for each possible time
    each page can contain a value, and notes
    for each order (s, d, p) :
        write a note in page [s]   : {RENT [d] [p]}
        write a note in page [s+d] : {CASH}
        
    Computing the value

    start with V = 0
    run through each page [t] of the planner in chronological order
    if page [t] contains a {CASH} note:
        update V with the value in the page if the value is greater than V (no value = 0)
    if page [t] contains a {RENT [d] [p]} note:
        update V with the value in the page if the value is greater than V (no value = 0)
        in page [t+d] write the value [V+p] if [V+p] is greater than that the value already at that page

Let's try this algoritm with our example case. First, we plan our orders:

    order at 0 duration 5 100:
        note in page 0 : RENT 5 100
        note in page 5 : CASH
    order at 3 duration 7 140:
        note in page 3  : RENT 10 140
        note in page 10 : CASH
    order at 5 duration 9 80:
        note in page 5  : RENT 14 80
        note in page 14 : CASH
    order at 6 duration 9 70:
        note in page 6  : RENT  15 70
        note in page 15 : CASH

    
    +----------+----------+----------+----------+----------+----------+----------+
    |     0    |     3    |     5    |    6     |    10    |    14    |    15    |
    +----------+----------+----------+----------+----------+----------+----------+
    |         0|         0|         0|         0|         0|         0|         0|
    |RENT 5 100|RENT 7 140|CASH      |RENT 15 70|CASH      |CASH      |CASH      |
    |          |          |RENT 15 80|          |          |          |          |
    +----------+----------+----------+----------+----------+----------+----------+

Then we run through the planner:

    starting with V = 0
    at page 0:
    V     ← max(V,P(0)) = 0
    P(5)  ← max(P(5),V+100) = 100
    at page 3:
    V     ← max(V,P(3)) = 0
    P(10) ← max(P(10),V+140) = 140
    at page 5:
    V     ← max(V,P(5)) = 100
    V     ← max(V,P(5)) = 100
    P(14) ← max(P(14),V+80) = 180 
    at page 6:
    V     ← max(V,P(6)) = 100
    P(15) ← max(P(15),V+70) = 170 
    at page 10:
    V    ← max(V,P(10)) = 140 
    at page 14:
    V    ← max(V,P(14)) = 180 
    at page 15:
    V    ← max(V,P(15)) = 180 

And V is now the maximum profit value we can draw from the orders. 

            V=0        V=0      V=100      V=100      V=140      V=180      V=180
    +----------+----------+----------+----------+----------+----------+----------+
    |     0    |     3    |     5    |    6     |    10    |    14    |    15    |
    +----------+----------+----------+----------+----------+----------+----------+
    |         0|         0|       100|         0|       140|       180|       170|
    |RENT 5 100|RENT 7 140|CASH      |RENT 15 70|CASH      |CASH      |CASH      |
    |          |          |RENT 15 80|          |          |          |          |
    +----------+----------+----------+----------+----------+----------+----------+


3. A Divide and Conquer Approach
--------------------------------

We want to solve this problem in Forth, using gforth. Let's first decompose our rather big problem into smaller ones:

1. defining and updating a global money value V 
2. retrieving and updating values in a table P that maps time values to money values 
3. for each order entered by the user, memorizing 2 actions in a list
4. sorting the list of actions by time then category of action (update, or update_and_rent)
5. executing all the actions in the sorted list

Defining a money value is very easy:

	VARIABLE VALUE
	0 VALUE !

Let's start with a simple proof of concept: we will pretend for a moment that start time can only be comprised betmeen 0 and 100 as well as duration time. This allow for our profit planner to reside in the dictionnary:

    200 CONSTANT MAX-TIME
    CREATE PROFIT MAX-TIME CELLS ALLOT
    PROFIT MAX-TIME CELLS ERASE   

    : PROFIT@ ( t -- m   finds profit value at time t or 0 )
        CELLS PROFIT + @ ;
         
    : PROFIT! ( m t --   stores profit value at time t )
        CELLS PROFIT + ! ;

Let's try our table:

	42 PROFIT@ .    ⏎  0 ok
	4807 42 PROFIT! ⏎  ok
	42 PROFIT@ .    ⏎  4807 ok

Now we need definition for actions:

	: CASH ( t --   update value with profit at time t if greater )
		PROFIT@ VALUE @ MAX
		VALUE ! ;

    : UPDATE-PROFIT ( p t -- update profit at time t with p if p is greater )
        DUP PROFIT@ 
        ROT MAX 
        SWAP PROFIT! ;
         
    : RENT ( s d p  -- update profit table at s+d for price p )
        ROT DUP CASH
        SWAP VALUE @ +
        -ROT + UPDATE-PROFIT ;
	
Let's ignore for now the problem of storing action events and retrieving them in order, and pretend we can just execute these actions in the right order:

    0 5 100 RENT
    3 7 140 RENT
    5       CASH
    5 9 80  RENT
    6 9 70  RENT
    10      CASH
    14      CASH
    15      CASH
    VALUE ? ⏎  180 ok

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

Enters [`act`](http://irdvo.nl/FFL/docs/act.html) , a module from the Forth Foundation Library. This module provides us with the ability to store key/values in AVL trees. Here we use `ACT-CREATE` to create a new AVL tree named `PROFIT`, then `ACT-INSERT` to insert a new node, and we retrieve a money$ value via its time key with `ACT-GET`:


	REQUIRE ffl/act.fs

	ACT-CREATE PROFITS

    : PROFIT@ ( t -- m   finds profit value at time t or 0 )
        PROFIT ACT-GET 0= IF 0 THEN ;
         
    : PROFIT! ( m t --   stores profit value at time t )
        PROFIT ACT-INSERT ;

	4807 500000 PROFIT! ⏎
	ok
	500000 PROFIT@ . ⏎
	4807  ok
	234 PROFIT@ . ⏎ 
	0  ok 

One very useful feature of `act` library is the ability to execute a given definition on each and every node of a given tree. The execution sequence is sorted by key value. Let's print all the profit values from our example:

    : .PROFIT ( m t --   pretty print the values )
        ." Profit[ " . ." ] = " . CR ;

    ' .PROFIT ( address of the defn ) PROFIT ACT-EXECUTE  ⏎
	Profit[ 5 ] = 100
	Profit[ 10 ] = 140
	Profit[ 14 ] = 180
	Profit[ 15 ] = 170
	Profit[ 500000 ] = 4807
	ok

5. Storing and sorting actions
------------------------------

Wait a minute: if an AVL tree can store values corresponding to keys and can be searched in order of its keys, this structure is also exactly what we need to store and sort actions! 

We just have to decide what will be the representation of the key and the representation for the data. Since the spec allows for distinct orders to have the same start time and duration, we need to have these two informations in our key, and we need a way to tell an update action from an update_and_rent action. That last information is actually easy to represent: if the duration part is 0 then the action is a simple update, if duration is > 0 then the action is an update_and_rent action.

Since a key is a 64 bits value, we can split that key in two 32 bits parts (which gives us more than enough room for our start time and duration values). Here are the definitions:

    : MASK ( b -- m  creates a mask of 32 bits all set to 1 ) 
        -1 SWAP RSHIFT ;

    : ACTION>KEY ( t d -- k   encode time and duration in a word value )
        SWAP 32 LSHIFT OR ;

    : KEY>ACTION ( k -- t d   decode time and duration from a word value )
        DUP 32 RSHIFT 
        SWAP 32 MASK AND ; 

And here's a test:

    5    9 ACTION>KEY DUP CR ." Key:" . KEY>ACTION SWAP ." Time:" . ." Duration:" . CR ⏎
	Key:21474836489 Time:5 Duration:9 
	ok
    4807 0 ACTION>KEY DUP CR ." Key:" . KEY>ACTION SWAP ." Time:" . ." Duration:" . CR ⏎
    Key:20645907791872 Time:4807 Duration:0
	ok  




  
