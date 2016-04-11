## 1) Processing renting events using a planner

Suppose we have the following events:

- at time 0, plan a rent until time 5 with bid = 10
- at time 3, plan a rent until time 10 with bid = 14
- at time 5 collect maximum value
- at time 8, plan a rent until time 12 with bid = 7
- at time 10 collect maximum value
- at time 12 collect maximum value

We start with *value* = 0, then process each event:

- at time 0, write 0+10 at position 5 in the plan
- at time 3, write 0+14 at position 10 in the plan
- at time 5, update *value* with max between *value* and *plan[5]*, i.e 10
- at time 8, write 10+7 at position 12 in the plan
- at time 10, update *value* with max between *value* and *plan[10]*, i.e 14
- at time 12, update *value* with max between *value* and *plan[12]*, i.e 17

At the end of the process, *value* will contain the maximum value possible with the plan.

The maximum orders we can have is 10000, and time positions can be any integer between 0 and 1000000, using an array would be a waste of space. Instead we use a [hash table](http://irdvo.github.io/ffl/docs/hct.html) in which keys are string and associated values are integer (max = 2^64).

*running* an event is simple:

    if the event type is *plan* 
        plan[end-time] = max(plan[end-time],value+bid)
    else ( it is *cash* )
        value = max(value, plan[event-time])


## 2) Generating events from orders
Given these orders:

- 0 5 10
- 3 7 14
- 8 4 7

we should generate these events:

- at time 0 rent until 5 for 10
- at time 5 collect cash
- at time 3 rent until 10 for 14
- at time 10 collect cash
- at time 8 rent until 12 for 7
- at time 12 collect cash

## 3) arranging event information for sorting of events

we want to have events sorted by event-time, then event-type (with *cash* event first). instead of writing our own sort procedure, we can use an [avl tree](http://irdvo.github.io/ffl/docs/act.html). Because the avl tree stores unique keys with one cell data associated to each key, we need each event to be unique, using all the event information (that way, two equivalent orders will insert only 2 events, not 4, in the avl tree). We also need the avl tree to return events sorted by time then type. 

event information is thus arranged as below (e.g. events *4807 cash* and *4807 plan for 4817,10*) :

    type   :      event key         
    RENT   : start-time | end-time | bid       
    CASH   | end-time   | 0        | 0 
   
we can compound and store several fields in a cell by mutliplying by powers of 10 and adding :

    start-time * 100000 + end-time * 100000 + bid

or we can using more efficient binary shifts :

    (((start-time << 24)|end-time)<<24)|bid

In either case we will need procedure to compose event key and decompose event key. The avl tree will associate no data with each key, as all the necessary info is in the key.

## 4) Rent your airplane and make money! 

The main routine is simple:

- intialize all variables and collections
- add orders (for each order adding events in the AVL tree)
- process all events in order iterating the AVL tree
- display the computed value



