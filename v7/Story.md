Rent your Airplane and Make Money
=================================
1. The Problem to Solve
-----------------------

###1.1 A Request

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


###1.2 A Formula

We can determine the profit of a list of N orders using this formula

- *R = maximum { P(i),P(i+1),..P(n) }*
- *R(i) = p(i) + maximum { P(j) | j=1..n, t(j) >= t(i)+d(i) }*

where t(i),d(i) and p(i) = the start time, duration and price for an order(i)

Applied to the example above:

- *R = maximum { P(1),P(2),P(3),P(4) }*
- *P(1) = 100 + maximum { P(3),P(4) }*
- *P(1) = 100 + maximum { 80 + maximum { }, 70 + maximum { } }*
- *P(1) = 180*
- *P(2) = 140 + maximum { }*
- *P(3) = 80 + maximum { }*
- *P(4) = 70+ maximum { }*
- *R = 180*

Infortunately this formula is not practical as the computation would require N! comparisons.

Another way to compute the solution is to define *P(t)*, the profit value at time *t*, and consider four rules:

1. *P(0) ≥ 0*
2. *∀ t,t'  t'>t ⇒  P(t') ≥ P(t)*
3. *∀ t,d>0,p>0 | Order(t,d,p) ⇒ P(t+d) ≥ P(t)+p*
4. *R = P(t) | t = maximum { s+d | Order(s,d,p) }*
    
Let's apply those rules to the case given in the request as an example:

- *Order(0,5,100) ⇒ P(5)  ≥ P(0)+100* (r.5)
- *P(0) ≥ 0       ⇒ P(5)  ≥ 100* (r.1)
- *Order(3,7,140) ⇒ P(10) ≥ P(3)+140*
- *P(3) ≥ P(0)    ⇒ P(10) ≥ 140*
- *Order(5,9, 80) ⇒ P(14) ≥ P(5)+80*
- *P(5) ≥ 100     ⇒ P(14) ≥ 180*
- *Order(6,9, 70) ⇒ P(15) ≥ P(6)+70*
- *P(6) ≥ P(5)    ⇒ P(15) ≥ 170* (r.2)
- *P(15) ≥ P(14)  ⇒ P(15) ≥ 180* (r.2)
- *R = P(15)      ⇒ R ≥ 180*

### 1.3 An Algorithm

Using the four rules suggests the following algorithm for solving our problem:

1. Planning the orders
    - using a planner with a page for each possible time
    - each page can contain a value, and notes
    - for each order (s, d, p) :
        - write a note in page [s]   : {RENT [d] [p]}
        - write a note in page [s+d] : {CASH}
        
2. Computing the value
    - start with R = 0
    - run through each page [t] of the planner in chronological order
    - if page [t] contains a {CASH} note:
        - update R with the value in the page if the value is greater than R (no value = 0)
    - if page [t] contains a {RENT [d] [p]} note:
        - update R with the value in the page if the value is greater than R (no value = 0)
        - in page [t+d] write the value [R+p] if [R+p] is greater than that the value already at that page

And P will containt the maximum profit value for the given plan.

Let's try this algoritm with our example case. First, we plan our orders:

 - order at 0 duration 5 100:
     - note in page 0 : RENT 5 100
     - note in page 5 : CASH
 - order at 3 duration 7 140:
     - note in page 3  : RENT 10 140
     - note in page 10 : CASH
 - order at 5 duration 9 80:
     - note in page 5  : RENT 14 80
     - note in page 14 : CASH
 - order at 6 duration 9 70:
     - note in page 6  : RENT  15 70
     - note in page 15 : CASH

The planner should look like this:

    +----------+----------+----------+----------+----------+----------+----------+
    |     0    |     3    |     5    |    6     |    10    |    14    |    15    |
    +----------+----------+----------+----------+----------+----------+----------+
    |         0|         0|         0|         0|         0|         0|         0|
    |RENT 5 100|RENT 7 140|CASH      |RENT 15 70|CASH      |CASH      |CASH      |
    |          |          |RENT 15 80|          |          |          |          |
    +----------+----------+----------+----------+----------+----------+----------+

Then we run through the planner:

- starting with R = 0
- at page 0: R ← max(R,P(0)) = 0 ; P(5)  ← max(P(5),P+100) = 100
- at page 3: R ← max(R,P(3)) = 0 ; P(10) ← max(P(10),P+140) = 140
- at page 5: R ← max(R,P(5)) = 100 ; R ← max(R,P(5)) = 100 ;  P(14) ← max(P(14),P+80) = 180 
- at page 6: R ← max(R,P(6)) = 100 ; P(15) ← max(P(15),P+70) = 170 
- at page 10: R ← max(R,P(10)) = 140 
- at page 14: R ← max(R,P(14)) = 180 
- at page 15: R ← max(R,P(15)) = 180 

And R is now equal to the maximum profit value we can draw from the orders: 

            R=0        R=0      R=100      R=100      R=140      R=180      R=180
    +----------+----------+----------+----------+----------+----------+----------+
    |     0    |     3    |     5    |    6     |    10    |    14    |    15    |
    +----------+----------+----------+----------+----------+----------+----------+
    |         0|         0|       100|         0|       140|       180|       170|
    |RENT 5 100|RENT 7 140|CASH      |RENT 15 70|CASH      |CASH      |CASH      |
    |          |          |RENT 15 80|          |          |          |          |
    +----------+----------+----------+----------+----------+----------+----------+


3. A Divide and Conquer Approach
--------------------------------

We want to solve this problem in Forth, using gforth. Let's first decompose our problem into smaller ones:

1. storing, retrieving and updating values in a table that maps time values to money values 
2. each time an order is entered, keeping track of actions {CASH} or {RENT} to perform 
3. sorting the list of actions by time then category {CASH} or {RENT}
4. executing all the actions in the sorted list

###3.1 Mapping time values to money values

Let's start with a simple proof of concept. We will pretend for a moment that the *start time* can only be comprised betmeen 0 and 100, as well as *duration*. That means that the maximum time value is 200. This allow for our profit planner to reside in the dictionnary:

    200 CONSTANT MAX-TIME
    CREATE PLAN MAX-TIME CELLS ALLOT
    PLAN MAX-TIME CELLS ERASE   

`CELLS` multiplies the number on the stack with the number of bytes in a cell (8 on my version of gforth). The memory for `PLAN` is alloted and filled with 0. We define access words:

    : PLAN@ ( t -- n   find profit at time t, or 0 )
        CELLS PLAN + @ ;
         
    : PLAN! ( n t --   store n in profit table at time t )
        CELLS PLAN + ! ;

Let's try our table:

	42 PLAN@ CR .    4807 42 PLAN!    42 PLAN@ CR .  ⏎
    0 
    4807 ok

Let's ignore for now the problem of interpreting orders as action marks to be sorted, and focus on the definitions we need to compute the maximum value:

    VARIABLE PROFIT

    : UPDATE-PROFIT ( n -- update value with n if n is greater )
        PROFIT @ 
        MAX
        PROFIT ! ;

    : UPDATE-PLAN ( n t -- update profit at time t with n if n is greater )
        DUP PLAN@ 
        ROT MAX 
        SWAP PLAN! ;

	: CASH ( t --   update value with profit at time t if greater )
		PLAN@ 
        UPDATE-PROFIT ;

    : RENT ( t d n  -- cash profit at time t, then update profit at t+d with PROFIT+p )
        ROT DUP CASH
        SWAP PROFIT @ +
        -ROT + 
        UPDATE-PLAN ;
	
Armed with these definitions we can pretend to run through a planner by executing actions in the right order:
    
    0 PROFIT !

    0 5 100 RENT
    3 7 140 RENT
    5       CASH
    5 9 80  RENT
    6 9 70  RENT
    10      CASH
    14      CASH
    15      CASH

    PROFIT ? ⏎  180 ok

It works!


4. Mapping time to money
------------------------

Of course, the specs for the requested program mention that time values can be as large as 2000000, so our solution of storing the PLAN table in the dictionary won't work:

	2000000 CONSTANT MAX-TIME  ok
	CREATE PLAN MAX-TIME CELLS ALLOT
	:2: Dictionary overflow
	CREATE PLAN MAX-TIME CELLS >>>ALLOT<<<
	Backtrace:
	$10568E8E0 throw

Besides, using such large dictionary space for only 10000 time point entries at last would be wasteful.

Enters [`act`](http://irdvo.nl/FFL/docs/act.html) , a module from the Forth Foundation Library. This module provides us with the ability to store key/values in AVL trees. Here we use `ACT-CREATE` to create a new AVL tree named `PLAN`, and then rewrite our access words so that they use `ACT-INSERT` to insert a new profit node, and `ACT-GET` to retrieve a profit at a given time :


	REQUIRE ffl/act.fs

	ACT-CREATE PLAN

    : PLAN@ ( t -- n   finds profit at time t or 0 )
        PLAN ACT-GET
        0= IF 0 THEN ;
         
    : PLAN! ( n t --   stores profit at time t )
        PLAN ACT-INSERT ;

    PLAN ACT-INIT
	500000 PLAN@ .
	4807 500000 PLAN!
	500000 PLAN@ . ⏎
	0 4807  ok

One very useful feature of `act` library is the ability to execute a given definition on each and every node of a given tree. The execution sequence is sorted by key. Let's print all the profit nodes from our example:

    : .PLAN ( m t --   pretty print the values )
        ." Plan[ " . ." ] = " . CR ;

    ' .PLAN ( address of the defn ) PLAN ACT-EXECUTE  ⏎
	Plan[ 5 ] = 100
	Plan[ 10 ] = 140
	Plan[ 14 ] = 180
	Plan[ 15 ] = 170
	Plan[ 500000 ] = 4807
	ok

5. Storing and sorting actions
------------------------------

If an AVL tree can store values corresponding to keys and can be searched in order of its keys, this structure is also exactly what we need to store and sort actions! 

We just have to decide what will be the representation of the key and the representation for the data. Clearly the key to each node will be a compound key comprising start time and duration since we don't want two different actions to reside in the same node of the tree. We can expect that some order will have equal start time and duration, and a different price: in that case, a node should be replaced only if the price is greater than the price already in the node.

Distinguishing between a {RENT} action and a {CASH} action is important because for a given time we should always perform the {CASH} action before any {RENT} action (e.g we need to update the cash value at time 5, before calculating the profit made at time [5+9]). Fortunately this information is already in the key, and the right order is guaranted, too:

- A {CASH} action has its key defined by *start time* and 0 as duration.
- A {RENT} action key is defined by *start time* and a *duration* which will always be > 0


Since a key is a 64 bits value, we can split that key in two 32 bits parts (which gives us more than enough room for our start time and duration values). Here are the definitions to encode time and duration into a key and vice-versa:

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

Now, storing actions is easy. We need an AVL tree that will serve as a key/value association tree:

    ACT-CREATE ACTIONS

To store a Cash action, we create a compound key with the time and a duration of zero, define a value of also zero, and insert that in the tree:
 
    : {CASH} ( t -- stores a cash action event in the action tree )
        0 ACTION>KEY
        0 SWAP
        ACTIONS ACT-INSERT ;

Storing a Rent action is a bit more work, because if a Rent action for the same time and duration is already present in the tree, its price should be replaced only with a greater price, not smaller.

So we create the compound key for the given time and duration, and then we search the tree for that key; if it's already present, we compare the price found  to the price we want to insert and keep the greater value. If it's not present, the price and the key are ready to be inserted in the tree.


    : {RENT} ( t d p -- store/update a rent action event in the action tree )
        -ROT
        ACTION>KEY DUP 
        ACTIONS ACT-GET IF
            ROT MAX SWAP 
        THEN 
        ACTIONS ACT-INSERT ;

Let's test our definitions. Here's a word that will print an action as retrieved from the tree: 

    : .ACTION ( n k -- pretty print an action read in the action tree )
        KEY>ACTION
        CR
        DUP 0= IF DROP . ." Cash " DROP 
        ELSE SWAP . . . ." Rent " THEN ; 

We initialize the tree, then insert some cash and rent actions:

    ACTIONS ACT-INIT
    5 9 100 {RENT}
    3 7 140 {RENT}
    5 4 200 {RENT}
    3 7  40 {RENT}
    5 {CASH}
    3 {CASH}

These actions are given in random order; several cases are given:

    - same time, different category
    - same time and category, different duration
    - same time, category and duration, different price

Then we print each node in the tree:

    ' .ACTION ACTIONS ACT-EXECUTE ⏎
	
	3 Cash
	3 7 140 Rent
	5 Cash
	5 4 200 Rent
	5 9 100 Rent	

And here are our actions sorted by time. Note that the rent at 3 to 7 for 40 has been replaced by a better one as expected.
We're almost done!

6. Entering Orders, Executing Actions
-------------------------------------


The algorithm takes two stages:
    
1. Planning the orders
2. Computing the profit value

To plan the orders, we will enter the start time, duration and price, then call the `ORDER` word. This will have the effect of:

- creating a cash action event at time = start + duration
- creating a rent action event at time = start with  duration and price parameters  

    : ORDER ( t d p -- store cash and rent actions for order t d p )
        -ROT 2DUP + {CASH}
        ROT {RENT} ;

Let's try our word:

    ACTIONS ACT-INIT
    5 9 100 ORDER
    3 7 140 ORDER
    5 4 200 ORDER
    3 7  40 ORDER

    ' .ACTION ACTIONS ACT-EXECUTE ⏎
	
	0 5 100 Rent
	3 7 140 Rent
	5 Cash
	5 9 80 Rent
	6 9 70 Rent
	10 Cash
	14 Cash
    
It works! Now for the computing part. Here's a word that will execute an action as retrieved from the `ACTIONS` tree:

    : CASH? ( d -- b  return true if action is Cash ie duration d is 0, false if Rent )
        0= ;

    : ACTION ( n k -- perform action defined by key and value )
        KEY>ACTION
        DUP CASH? IF CASH DROP
        ELSE SWAP RENT THEN ;

And now we have to execute this word on every action stored in the tree:

    ' ACTION CONSTANT EXEC-ACTION

    : COMPUTE-PROFIT ( compute the profit value for all given orders )
        0 PROFIT !
        PLAN ACT-INIT
        EXEC-ACTION ACTIONS ACT-EXECUTE ; 

Let's test our program:

    ACTIONS ACT-INIT
    0 5 100 ORDER
    3 7 140 ORDER
    5 9  80 ORDER
    6 9  70 ORDER
    COMPUTE-PROFIT
    CR PROFIT ? ⏎
	180 Ok

Fantastic! It works!

        




  
