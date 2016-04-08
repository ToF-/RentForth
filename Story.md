# 1. Simple cases

If we had only one order, there would be no need for a word to compute the rent value of that order. Take the number at the top of the stack and you're done. 

    10
    .
    10 ok

If we had two orders



We will store the rent value calculated by our program into a variable:

    VARIABLE RENT-VALUE
    ok

After we add orders, and compute the value, the variable should contain the best value possible given these orders. If we had only one order, getting the rent value should be very simple: we store the bid of the order into our variable, and get rid of the start and duration.

    : ADD-ORDER ( start duration bid -- )
        RENT-VALUE ! 2DROP ;
    ok

For example:

    0 5 10 ADD-ORDER
    RENT-VALUE ?
    10 OK

If we had 2 conflicting orders, the rent value should be set to the max of the two orders :

    : ADD-ORDER ( start duration bid -- )
        RENT-VALUE @ MAX RENT-VALUE ! 2DROP ;
    redefined ADD-ORDER ok

For example:

