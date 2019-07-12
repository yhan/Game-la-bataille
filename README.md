
1. **Egalité**  
en cas d’égalité sur bataille, on répète la procédure de bataille. Le premier qui n’a plus de carte perd. Si les deux n’ont plus de carte en même temps, il y a match nul.

1. **Fin de jeu**  
Pour la fin de jeu, effectivement, il y a des cas ou ça peut ne jamais se terminer. Ce que je peux te conseiller est de mélanger les cartes gagnées (dans la vraie vie, en ramassant les cartes, on les ramasse rarement toujours dans le même ordre). A cela peut être ajouté une règle : « après 1000 tours dans une partie, les joueurs s’arrêtent et il y a match nul. »

1. **Deux joueurs qui reste dans le jeu n'a plus qu'une carte chacun, et qu'il y bataille**
Ce qui a la plus grande carte gagne (on met plus deux carte sur le plateau. Une face caché une autre face découverte)

1. **Console**  
Pour la console, pas besoin de pauses.


# Run my program in your Unit tests

Some decision on my side can impact the test result:  

t4 for _trefle de quatre_; c2 for _coeur de deux_

1.  If my initial stack is: t4, c2 (t4 on the bottom of `paquet`) and the guy wins two cards c5 and p13. To introduce some determinism to the game, I decide the way I put the won cards on the bottom of my `Paquet` is: put the smaller on the bottom and bigger one on bottom + 1 level.
If the values of cards are identical, refer to `figure`, the order of `figure` follows the value of `enum Figure` in the code base.


# Domain specific language
As the text is in French, the wording for domain is in French as well. On the technical words remain English.

# Reference

[This](https://www.fetedujeu.org/jeux-societe/cartes/bataille/) gave me some clarification of some implicit points.