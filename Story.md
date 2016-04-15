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

Applied to the example above:

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

So what we have to do rather than search the order list for the maximum value recursively, is to process each calculation at the right time, while keeping track of the profit made so far. Each order O(t,d,p) involve two actions:

- at time t, rent the airplane, thus writing a profit increase by p at time t+d
- at time t+d, update de profit made so far

    Order(t,d,p) ⇒ A(t, P[t + d] := max(V, P[t] + p)), A(t + d, V := max(V, P[t + d]))

If we take all the actions generated through each order, and execute them in the right sequencing, i.e for each time position

1. update the profit made so far
2. plan the ulterior profit made by rent at price + profit 

then we end up with the maximum profit made with all the orders.

3. First Approach
-----------------

Let's begin with a first implementation. To keep things simple, we will change one of the constraints in the initial problem:

1. we will deal with small time values only:
    - start time : 0 ≦ s < 50
    - duration   : 0 < d < 50 

2. simplified problem: we calculate the profit not from a list of orders but from an already sorted sequence of renting and updating actions.

We need a variable to store the current profit, and an array to store the plan. We will limit the capacity to 20 time slots, from 0 to 19.

    VARIABLE PROFIT 

Given that in gforth, a cell is 8 bytes long, the phrase 

    CREATE PLAN 20 CELLS ALLOT

will reserve 1600 bytes for an array named PLAN. The following word erases the array and set the profit to zero.

    : INITIALIZE PLAN 20 CELLS ERASE 0 PROFIT ! ;

Updating profit at a given time is done by taking the value in the `PLAN` arraytay a time t and storing that value into `PROFIT` if it's greater.

    : UPDATE-PROFIT ( time -- ) 
        CELLS PLAN + @  PROFIT @  MAX  PROFIT ! ;

Planning a rent for a given time, duration and price is done by adding the price to the profit made so far, and storing that value in the plan at the ending time, if that value is greater than the curent value. 

    : UPDATE-PLAN ( value time -- ) 
        PLAN +  DUP @  ROT MAX  SWAP ! ;

    : RENT-AIRPLANE ( time duration price -- )
        PROFIT @ +  -ROT +  UPDATE-PLAN ;

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

    CR PLAN 5 + @ . PLAN 10 + @ . PLAN 14 + @ . PLAN 15 + @ . ⏎ 
    100 140 180 170 ok

4. Storing the Plan in a AVL tree
---------------------------------

Of course this implementation is not suited for the constraints of the request, because creating a two millions cells array simply doesn't work:

    CREATE PLAN 2000000 CELLS ALLOT ⏎
    :1: Dictionary overflow
    CREATE PLAN 2000000 CELLS >>>ALLOT<<<
    Backtrace:
    $109FA98E0 throw

And even if it would, storing 20000 actions in an 2000000 cells array is a waste of memory. Instead we can use an associative container that will allow us to map profit values on time keys. We will use an [avl tree](http://irdvo.github.io/ffl/docs/act.html) to do that. Here's how it works:

    REQUIRE ffl/act.fs ⏎ ok
    ACT-CREATE PLAN ⏎ ok 
    1000 4807 PLAN ACT-INSERT ⏎  ok 
    4807 PLAN ACT-GET . . ⏎  -1 1000 ok
    3280 PLAN ACT-GET . ⏎  0  ok

As you can see, `ACT-GET` can have two different effects on the stack:
- if the container has the searched key, a true flag (-1) will be at the top of the stack, then value associated to the key.
- if the container doesn't have the key, a false flag (0) will be at the top of the stack.

So let's refactor our program, starting with importing the ACT library:

    REQUIRE ffl/act.fs

We declare the plan and the profit variable:

    VARIABLE PROFIT 
    ACT-CREATE PLAN

We have to intialize the plan, freeing the avl nodes from a possible previous use:

    : INITIALIZE PLAN ACT-(FREE) 0 PROFIT ! ;

Fetching a profit value is now done through the avl tree. If the node for a given time can't be found, the value returned should be zero.

    : PLAN@ ( time -- value )
        PLAN ACT-GET 0= IF 0 THEN ;

Storing a profit value is also done using the avl tree:

    : PLAN! ( value time -- )
        PLAN ACT-INSERT ;

And now we can adapt the action words:

    : UPDATE-PROFIT ( time -- ) 
        PLAN@ PROFIT @  MAX  PROFIT ! ;

    : UPDATE-PLAN ( value time -- )
        DUP PLAN @  ROT  MAX  SWAP PLAN! ;

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
###5.1 Traversing an AVL Tree 
We have now solved the problem of respecting the data size constraint, but for our program to be complete, we still need to generate actions from orders, and to *sequence* these actions so that for a given time value, `UPDATE-PROFIT` will be executed before `RENT-AIRPLANE`. Thus we need to somehow "store" actions into a collection-type structure, sort that structure according to the right criteria, and then execute all the actions in order.

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
###5.2 Creating composite keys from actions

The problem is now to encode the action information into one cell, creating at composite key that will respect the ordering of sequence that we need. How do we do that?

We know that for any order with start time t, duration d and price p, there should be

* a plan-rent action at time t, with 2 parameters t+d and p
* an update-profit action at time t+d, with parameter p = 0

One way to represent many information in a cell value so that the cell value will respect a given order is to multiply the "major" part of the key by a factor greater than the greatest possible "minor" part of the key. For example if we want to store a date D(d,m,y) in a single value k that repects chronological order we can use k = y*1000 + m * 100 + d. For instance:

    - 20151231 = December 31 15
    - 20160331 = March 31 16
    - 20160401 = April 1  16

We could apply this method to create our action cell value, for example a plan rent action at 3 with duration 7 for price 140 could be encoded as k t * 10^12 + d * 10^5 + p =  3000007000140

But a more efficient is to use binary left shifting instead of multiplication : k = t << 38 || d << 17 || p, and the result will be the same: different action values will be encoded in a way to be ordered by time of action, then duration then price. 

The way to encode a value in a cell on n bits is first to shift left the cell by n bits, then do an or between the cell and the value.

    : <FIELD ( value cell #bits -- cell' )
        LSHIFT OR ;

For example, suppose we want to store a value of 3 in the 4 lower bits of a cell that has value 1:

    3 1 4 <FIELD . ⏎  19 ok

What's interesting in that way of ordering the parameters on the stack is that we can encode several values in a cell with a series of`<FIELD` operations. For example if we want to store the values 1 2 3 into 3 8-bits fields we write:

    3 2 1  8 <FIELD  8 <FIELD  HEX . ⏎  10203 ok

For our action cell, we need the price value to go on the 17 lowest bits, then the duration value to go on the next 21 bits, then the start time on the highest bits. The (intermediate) `(ACTION>KEY)` will do the work, with definied constants for clarity:

    21 CONSTANT LONG
    17 CONSTANT SHORT

    : (ACTION>KEY) ( time duration price -- key )
        SWAP ROT  LONG <FIELD  SHORT <FIELD ;  

If we try this and print the result in binary, we can see how the value 3 ( 11 ), 7 (111) and 14 (1110) were stored on the key:

    3 7 14 (ACTION>KEY) 2 base ! CR .  ⏎  
    1100000000000000000011100000000000001110 ok

The way we will encode rent-airplane actions and update-profit actions is different: 

- rent-airplane actions should be encoded as t,d,p
- update-plan actions should be encoded as t+d,0,0

This is based on the following rules :

- for a given time, the update-plan action always precede any rent-airplane action.
- an update-plan action doesn't have a price parameter.

Consequently, the word `ACTION>KEY` will first check for the type of action, reset the parameters to 0 for a update-plan action, and then execute the helper word:

    : ACTION>KEY ( time duration price -- key )
        ?DUP 0= IF + NIL NIL THEN (ACTION>KEY) ;

If we try to encore an update-plan action (t,d,0) we can see that the key t+d = 10 (1010) has been stored, and the other parameters are 0:

    3 7 0 ACTION>KEY  2 base ! CR .  ⏎  
    101000000000000000000000000000000000000000  ok
