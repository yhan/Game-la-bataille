

# Solution structure
Provided solution uses c# on .net core platform.

1. Solution structure:
    - Algorithm in La-Bataille project
    - Console app in La-Bataille.Console
    - Tests project in La-Bataille.Tests

1. Dependencies:  
Algorithm does not use any third party libraries.
Third party libraries are used in test project. (cf. [Tests section](#tests))

# Domain language
After have hesitated a long time, I decided to use English. As mixing French noun and English verbs (technical or non technical)  is really weird.
For instance, "la bataille" is called `Battle` is the solution.

# Entities and Aggregate root used in solution
cf. c# code documentation

**Aggregate root**: 
- `Competition`: A competition is composed of N `Game`(N >= 1), also called `Rounds`  

**Entity**: 
- `Game`: In the game 2 or more players do the "la bataille" game. (The core logic is here)
- `Player`: The participant of Game.
- `Take`: Player takes a card, is called a Take.
- `Card`
- `TwoFaceCard`: Represent a card is played by a Player wih Face-Up or Face-down property.
- `CardStack`: Represent the card stack in front of each Player. Take from the top of stack;  Put back to the bottom of stack.
- `View`: Represent what we see on the table for a given time.


# Assumption

> __In the following document, let's following this:  We start from right (top of the card stack), in a configuration: Player A: 3,  9,  10, 8. The first card to play is 8.__

> __In the following document, Cards is numerated from 2 to 14. 11 is Jack, 12 Queen, 13 is King and 14 is As.__

1. **Drop cards**
In some circumstances of battle, I have no choice but drop cards, because we can't execute a battle for the following reasons:
   1. _**In a battle, one or no more player still has card.**_  
     Player A: (... no more card here) 11 (♣)  
     Player B: (... no more card here) 11 (♥)    
     The two jacks meet battle condition, but both players have no more card, hence can't put face-down cards. I drop the 2 jacks.   
     OR  
     Player A:            10(♣), 2(♣), 11 (♣)  
     Player B: (... no more card here) 11 (♥)    
     The two players can't make a battle, I drop th 2 jacks

   1. _**In a battle, battle competitors can no more take out face-up cards**_  
       player A:            8, 14  
       player B:            9, 14  
       player C: 7, 10, 11, 2,  3  

       A and B battle at 14, but they can no more cards to put face-up.  
       Difficult decision to make! If I give all involved cards to C, not fair play because at this round both A and B have higher cards than C. My arbitrage is to  `drop` all involved cards: 8, 9, 14, 14, 3.
   
    1. _**In a battle, only on battle competitor can take out face-up card**_  
     Player A:    Q(♣),  2(♣),  Jack (♣)  
     Player B:           10(♣), Jack (♥)    
     =>  I drop all 5 cards.


1. **Draw on battle**  
In case of a draw on battle, the battle procedure is repeated. The first one who does not have a card loses. If both have no card at the same time, there is a draw. (_indicated by Marc-Antoine_)

1. **End of game**  
For the end of the game, there are cases where it can never end.  
What is implemented is do 1000 iterations, if no winner, then all survivors shuffle their earned cards. After the shuffling, after 1000 iterations still got no winner, then the players stop and there is a draw." (_suggested by Marc-Antoine_)

1. **Triggering condition of Battle**
When we have more than 2 players, battle will be triggered only when 2 or more than 2 players have the **strongest card**.
i.e if we have the following situation:  
Player A: 3,  **9**,  10, 8  
Player B: 4,  **7**,  11, 8  
Player C: 5,  **7**,  12, 8  

The battle starts from the identical eights.   
10, 11, 12 face down; 9, 7, 7 face up. Here the battle won't be triggered, as the strongest card is 9, even that B and C have the same card 7. 
> I find the rule [here](https://www.jeux-cartes.biz/jeux-daccumulation/bataille/) : __Si deux ou plusieurs joueurs sont à égalité pour le plus haut il y a une bataille.__

On the contrarily, if we have:
_Player A_: 3,  **6**,  10, 8  
_Player B_: 4,  **7**,  11, 8  
_Player C_: 5,  **7**,  12, 8  

Then yes, we have a battle with 6, 7 and 7, as 7 is the strongest card in this round.

1. **Deal with situation of battle**

    1. **Deal with the players who are not in the battle**

        player A:       2, 1,  7  
        player B:       3, 10, 7  
        player C: 3, 4, 5, 11, 2  

        Obviously we have battle between A and B, and the winner of battle is B.
        __The question is how to deal with player C's `card 2`, which is uncovered (face up)?__.
        In my solution, player B can gather also that `card 2 `. So finally, battle allows B to take 2, 1, 7 (A's cards), 3, 10, 7 (B keeps his own cards) and 2 (C's card)

1. **In what order should put the earned cards back to stack?**
   If my initial stack is: 4, 2 (t4 on the bottom of `CardStack`) and the guy wins two cards 5 and 13. To introduce some determinism, I decide the way I put the won cards in descendent sorted order.  

   So after have gathered the won cards, stack looks like: 5, 13, 4, 2
   If the values of cards are identical, refer to `figure`, the order of `figure` follows the value of `enum Figure` in the code base.


# Run my program in your Unit tests



# Reference

[This](https://www.fetedujeu.org/jeux-societe/cartes/bataille/) gave me some clarification of some implicit points.


BUG fix

 while (NeedBattle(takes.KeepTheLast(numberOfPlayersInTheGame), ref competitors))

Check if numberOfPlayersInTheGame is correct

2) numberOfPlayersInTheGame is correct?



# Original development requirement
**Goal**
Your program must create and play n virtual players in the "Battle" card game.

**Input**:
 - either a number of players and a number of games to play
 - or a list of players who have already had a distributed card package

**Output**:
 - The history of the cards having been seen on the board 
 - and the ranking of the players at the end of the games
