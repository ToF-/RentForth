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
    - start time st: 0 ≤ st \< 1000000
    - duration d: 0 \< d \< 1000000
    - price p: 0 \< p \< 100000


2. The Formula
--------------

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

So what we have to do rather than search the order list for the maximum value recursively, is to process each calcultation at the right time. Each order t,d,p generates two actions to process:

- at time t, renting the airplane, thus planning a profit increase by p, at time t+d 
- at time t+d, update the profit made so far 


    Order(t,d,p) ⇒ A(t, P[t + d] := max(V, P[t] + p)), A(t + d, V := max(V, P[t + d]))

If we take all these actions generated through each order, and execute them in the right sequencing, i.e for each time position

1. update the profit made so far
2. plan the ulterior profit made by rent at price + profit 

then we end up with the maximum profit made with all the orders.

3. First Approach
-----------------

Let's begin with a very simple implementation of this idea, removing or changing some of the constraints in the initial problem:

1. small numbers only:
    - start time : 0 ≦ s < 50
    - duration   : 0 < d < 50 
    - price      : 0 < p < 150
2. simplified problem: we calculate the profit not from a list of orders but from an already sorted sequence of renting and updating actions.

We need a variable to store the current profit, and an array to store the plan. We will limit the capacity to 20 time slots, from 0 to 19.

    VARIABLE PROFIT 

Given that in gforth, a cell is 8 bytes long, the phrase 

    CREATE PLAN 20 CELLS ALLOT

will reserve 1600 bytes for an array named PLAN. The following word erases the array and set the value to zero.

    : INITIALIZE PLAN 20 CELLS ERASE 0 PROFIT ! ;

Updating profit at a given time is done very simply:

    : UPDATE-PROFIT ( time -- ) CELLS PLAN + @ PROFIT @ MAX PROFIT ! ;

Planning a rent for a given time is simple too:

    : RENT-AIRPLANE ( price time -- ) CELLS PLAN + DUP @ ROT PROFIT @ + MAX SWAP ! ;

Let's try our words:

    INITIALIZE ⏎ ok
    100 5  RENT-AIRPLANE ⏎ ok 
    140 10 RENT-AIRPLANE ⏎ ok 
        5  UPDATE-PROFIT ⏎ ok 
     80 14 RENT-AIRPLANE ⏎ ok
     70 15 RENT-AIRPLANE ⏎ ok
        10 UPDATE-PROFIT ⏎ ok
        14 UPDATE-PROFIT ⏎ ok
        15 UPDATE-PROFIT ⏎ ok
    PROFIT ? ⏎ 180 ok
    
And this should convince us that using the plan worked correctly:

    PLAN 5 + @ . PLAN 10 + @ . PLAN 14 + @ . PLAN 15 + @ . ⏎  100 140 180 170 ok

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
    3280 PLAN ACT-GET . ⏎ 0  okw

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

    : UPDATE-PROFIT ( time -- ) PLAN@ PROFIT @ MAX  PROFIT ! ;
    : RENT-AIRPLANE ( time duration price -- )
         -ROT + DUP PLAN@               ( price end P[end] )
         ROT PROFIT @ +                 ( end p[end] profit+price )
         MAX SWAP PLAN! ;

And test the code with a new example involving large time values:

    INITIALIZE ⏎ ok
    1000  500000 RENT-AIRPLANE ⏎ ok
    1400 1000000 RENT-AIRPLANE ⏎ ok
          500000 UPDATE-PROFIT ⏎ ok
     800 1400000 RENT-AIRPLANE ⏎ ok
     700 1500000 RENT-AIRPLANE ⏎ ok
         1000000 UPDATE-PROFIT ⏎ ok
         1400000 UPDATE-PROFIT ⏎ ok
         1500000 UPDATE-PROFIT ⏎ ok
    PROFIT ? ⏎  1800 ok

5. Sequencing Actions
---------------------
6. The Main Program
-------------------


5. Generating sorted actions
----------------------------

We have now solved the problem of respecting the data size constraint, but for our program to be complete, we still need to generate actions from orders, and to *sequence* these actions so that for a given time value, `UPDATE-PROFIT` will be executed before `RENT-AIRPLANE`. Thus we need to somehow "store" actions into a collection-type structure, sort that structure according to right criteria, and then execute all the actions in order.

As it happens, an AVL tree can perfectly be traversed in the order defined by its key values. The word `ACT-EXECUTE` will do that for us, provided we give him *what* to execute. 

Suppose we have a tree with some keys-values associations in it:

    ACT-CREATE MY-TREE ⏎ ok

    1000 4807 MY-TREE ACT-INSERT ⏎ ok
    2000   24 MY-TREE ACT-INSERT ⏎ ok
    3000  352 MY-TREE ACT-INSERT ⏎ ok

and a word to print key value pairs:

    : .KEY-VALUE ( value key -- ) CR . ." => " . ; ⏎ ok

then we can pass the *execution address* of the word `.KEY-VALUE` and have our tree execute this word for all the keys, in order.
 
    ' .KEY-VALUE MY-TREE ACT-EXECUTE ⏎ 
    24 => 2000
    352 => 3000
    4807 => 1000 ok

The "tick" word ` ` ` is what makes this execution possible. 

How do we encode the actions into ordered keys? We know that for any order with start time t, duration d and price p, there should be

* a plan-rent action at time t, with 2 parameters t+d and p ready to be used by `RENT-AIRPLANE` 
* an update-profit action at time t+d, with no parameter needed because `UPDATE-PROFIT` will use t+d

So we need to create a *compound key* with the values t, t+d, p (for a plan-rent action) or the values t+d,t+d,0 (for an update-profit action). One way to do this is to multiply the major parts of the key :

* plan-rent:  K(t,d,p) = t * 100000000000 + (t+d) * 100000 + p (e.g. for 3 7 and 1400: 300001001400)
* update-profit: K(t,d,p) = (t+d) * 100000000000  (e.g. for 3 7 and 1400: 1000000000000) 

A more efficient way is to use binary left shifting instead of multiplication:

* plan-rent: K(t,d,p) = t << 38 | (t+d)<< 17 | p
* update-profit: K(t,d,p) = (t+d) << 38

Let's create words to do that:

    : RENT-ACTION ( time duration price -- key ) ROT 21 LSHIFT ROT OR 17 LSHIFT OR ;
    : UPDATE-ACTION ( time duration -- key ) + 38 LSHIFT ;

And the following example shows that the action key for updating at 3+5 is lower than the action key for renting at 8 even when duration and price are the lowest possible.

    3 5 1 RENT-ACTION CR . 3 5 UPDATE-ACTION CR .  8 1 1 RENT-ACTION CR . ⏎ 
    824634376193
    2199023255552
    2199023386625 ok

As much as we can encode actions, we should be able to decode actions from the keys that the AVL tree will sort for us.

