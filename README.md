
# Summary
**Goal**
Your program must create and play n virtual players in the "Battle" card game.

**Input**:
 - either a number of players and a number of games to play
 - or a list of players who have already had a distributed card package

**Output**:
 - The history of the cards having been seen on the board 
 - and the ranking of the players at the end of the games

# Assumption

1. **Tie on battle**  
In case of a tie on battle, the battle procedure is repeated. The first one who does not have a card loses. If both have no card at the same time, there is a draw.


1. **End of game**  
For the end of the game, there are cases where it can never end. What I can advise you is to mix the cards earned (in real life, by picking up the cards, we rarely pick them up in the same order). To this can be added a rule: "After 1000 rounds in a game, the players stop and there is a draw. "

1. **D  ne autre face découverte)

1. **Console**  
Pour la console, pas besoin de pauses.

1. **Deal with situation of battle**

    1. **Deal with the players who are not in the battle**

        player A:       2, 1,  7  
        player B:       3, 10, 7  
        player C: 3, 4, 5, 11, 2  

        obviously we have battle between A and B, and the winner of battle is B.
        __The question is how to deal with player C's `card 2`__.
        In my solution, player B can gather also that card. So finally, battle allows B to take 2, 1, 7 (A's cards), 3, 10, 7 (B keeps his own cards) and 2 (C's card)

    1. **Battle competitors have not enough cards to pull a face-up card**
       
       player A:            8, 14  
       player B:            9, 14  
       player C: 7, 10, 11, 2,  3  

       A and B battle at 14, but they can no more pull face-up cards.  
       Difficult decision to make! If I give all involved cards to C, not fair play because at this round both A and B have higher cards than C. My arbitrage is to  `drop` all involved cards: 8, 9, 14, 14, 3.

# Run my program in your Unit tests

Some decision on my side can impact the test result:  

t4 for _trefle de quatre_; c2 for _coeur de deux_

1.  If my initial stack is: t4, c2 (t4 on the bottom of `paquet`) and the guy wins two cards c5 and p13. To introduce some determinism to the game, I decide the way I put the won cards on the bottom of my `Paquet` is: put the smaller on the bottom and bigger one on bottom + 1 level.
If the values of cards are identical, refer to `figure`, the order of `figure` follows the value of `enum Figure` in the code base.


# Domain specific language
As the text is in French, the wording for domain is in French as well. On the technical words remain English.

# Reference

[This](https://www.fetedujeu.org/jeux-societe/cartes/bataille/) gave me some clarification of some implicit points.


