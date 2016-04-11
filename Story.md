Rent your Airplane and Make Money
=================================
1.  The request
---------------

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
    - start time st: 0 ≤ st \< 1000000
    - duration d: 0 \< d \< 1000000
    - price p: 0 \< p \< 100000

2. Finding the formula
----------------------

We can determine the profit of a list of N orders using this formula:o

M = maximum { P(i),P(i+1),..P(n) }

where
 
P(i) = price(i) + maximum { P(j) | j=1..n, start(j) >= start(i)+duration(i) }

Applied to the examplei above:

        M = maximum { P(1),P(2),P(3),P(4) }

        P(1) = 100 + maximum { P(3),P(4) }
             = 100 + maximum { 70 + maximum { }, 80 + maximum { } }
             = 180
        P(2) = 140  + maximum { }
        P(3) = 80  + maximum { }
        P(4) = 70  + maximum { }

Infortunately this formula is not practical as the computation time of P would be O(N!).

However, we know that 

    ∀ t,t' t'>t ⇒ P(t') ≥ P(t) 
    ∀ t,d>0,p>0 | Order(t,d,p) ⇒ P(t+d) ≥ P(t)+p  
    
Applied to our example:

    Order(0,5,100) ⇒ P(5)  ≥ P(0)+100 ⇒ P(5) ≥ 100
    Order(3,7,140) ⇒ P(10) ≥ P(3)+140 ⇒ P(10) ≥ 140
    Order(5,9, 80) ⇒ P(14) ≥ P(5)+80 ⇒ P(14) ≥ 180
    Order(6,9, 70) ⇒ P(15) ≥ P(6)+70 ⇒ P(15) ≥ 170
                     P(15) ≥ P(14) ⇒ P(15) ≥ 180 

So what we have to do rather than search the order list for the maximum value recursively, is to process each calcultation at the right time. Each order generates two actions to process:

- plan the renting at time t and updating the profit planned at t+d
- update the profit made so far at t+d

    Order(t,d,p) ⇒ A(t, P[t+d] := max(V,P[t]+p)), A(t+d, V := max(V,P[t+d]))

If we process all the actions in the right sequencing, i.e for each time position

- update the profit made so far
- plan the ulterior profit made by rent at price + profit 

then we end up with the max profit made with all the orders.

3. A first, partial implementation
----------------------------------

Let's begin with a very simple implementation of this idea, removing or changing some of the constraints in the initial problem:

- start time : 0 ≦ s < 50
- duration   : 0 < d < 50 
- price      : 0 < p < 150
- we will process actions, not orders, and they are already given in the right order

We need a variable to store the current profit, and an array to store the plan. We will limit the capacity to 100 time slots, from 0 to 99.

    VARIABLE PROFIT 

Given that in gforth, a cell is 8 bytes long, the phrase 

    CREATE PLAN 20 ALLOT

will reserve 800 bytes for an array named PLAN. The following word erases the array and set the value to zero.

    : INITIALIZE PLAN 20 ERASE 0 PROFIT ! ;

Updating profit at a given time is done very simply:

    : UPDATE-PROFIT ( time -- ) PLAN + C@ PROFIT @ MAX PROFIT ! ;

Planning a rent for a given time is simple too:

    : PLAN-RENT ( price time -- ) PLAN + DUP C@ ROT PROFIT C@ + MAX SWAP C! ;

Let's try our words:

    INITIALIZE ⏎ ok
    100 5  PLAN-RENT ⏎ ok 
    140 10 PLAN-RENT ⏎ ok 
        5  UPDATE-PROFIT ⏎ ok 
     80 14 PLAN-RENT ⏎ ok
     70 15 PLAN-RENT ⏎ ok
        10 UPDATE-PROFIT ⏎ ok
        14 UPDATE-PROFIT ⏎ ok
        15 UPDATE-PROFIT ⏎ ok
    PROFIT ? ⏎ 180 ok
    
And this should convince us that using the plan worked correctly:

    PLAN 5 + C@ . PLAN 10 + C@ . PLAN 14 + C@ . PLAN 15 + C@ . ⏎  100 140 180 170 ok

4. Using an associative container for actions
---------------------------------------------

This program works, but it is not suited for the constraints of the request, because creating a two millions cells array simply doesn't work:

    CREATE PLAN 2000000 CELLS ALLOT ⏎
    :1: Dictionary overflow
    CREATE PLAN 2000000 CELLS >>>ALLOT<<<
    Backtrace:
    $109FA98E0 throw

And even if it worked, only 10% of that array would be used, which is obviously inefficient. Instead we can use an associative container that will allow us to map profit values on time keys. We will use an [avl tree](http://irdvo.github.io/ffl/docs/act.html) to do that. Here's how it works:

    REQUIRE ffl/act.fs ⏎ ok
    ACT-CREATE PLAN ⏎ ok 
    1000 4807 PLAN ACT-INSERT ⏎ ok 
    4807 PLAN ACT-GET . . ⏎ -1 1000 ok
    3280 PLAN ACT-GET . ⏎ 0  ok

Replacing the `PLAN` array by an avl involves requiring the avl source code:

    REQUIRE ffl/act.fs

We declare the plan and the profit variable:

    VARIABLE PROFIT 
    ACT-CREATE PLAN

We have to intialize the plan, freeing the avl nodes from a possible previous use:

    : INITIALIZE PLAN ACT-(FREE) 0 PROFIT ! ;

Fetching a profit value is now done through the avl tree. If the node for a given time can't be found, the value returned should be zero.

    : PLAN@ ( time -- value ) PLAN ACT-GET 0= IF 0 THEN ;

Storing a profit value is also done using the avl tree:

    : PLAN! ( value time -- ) PLAN ACT-INSERT ;

And now we can adapt the action words:

    : UPDATE-PROFIT ( time -- ) PLAN@ PROFIT @ MAX PROFIT ! ;
    : PLAN-RENT ( price time -- ) DUP PLAN@ ROT PROFIT @ + MAX SWAP PLAN! ;

And test the code with our example data.
 

5. Generating sorted actions
----------------------------



