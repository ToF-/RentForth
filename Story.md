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

    INITIALIZE
    100 5  PLAN-RENT 
    140 10 PLAN-RENT 
        5  UPDATE-PROFIT 
     80 14 PLAN-RENT 
     70 15 PLAN-RENT 
        10 UPDATE-PROFIT 
        14 UPDATE-PROFIT 
        15 UPDATE-PROFIT 
    PROFIT ? 
    180 ok
    
And this should convince us that using the plan worked correctly:

    PLAN 5 + C@ . PLAN 10 + C@ . PLAN 14 + C@ . PLAN 15 + C@ .
    100 140 180 170 ok

4. Generating sorted actions
----------------------------

How do we get from a sequence of orders to a sorted sequence of action? We could follow a plan like this:

- for each order, generate and store the actions data and add this data into an array
- sort the array (using action time, then type of action as sort criteria, with update-profit < plan-rent) 
- traverse the array, executing all the action

Each action will encoded in such a way that for a given time, update-profit action will be "lower" than plan-rent action. This can be done by multiplying the different parts of the action data by 10^12 and 10^6:

- action(update profit at t) = t * 1000000000000  
- action(plan rent at t for d with price p) = t * 1000000000000 + (t+d) * 1000000 + p
 
Those two words will encode actions:

    : ENCODE-RENT-ACTION ( price time duration -- code )
        OVER + SWAP ( price end time )
        1000000000000 * SWAP 1000000 * + + ;
 
    : ENCODE-UPDATE-ACTION ( time duration -- code )
        + 100000000000 * ;



    
    
    




