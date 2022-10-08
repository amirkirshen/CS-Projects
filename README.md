# Checkers Game

## Description:
Checkers (American English), also known as draughts (British English), is a group of strategy board games for two players which involve 
diagonal moves of uniform game pieces and mandatory captures by jumping over opponent pieces.                                                
This game is the american version of checkers (game's rules are below) using C#, Window Forms and .NET framework.                       
This project was developed as part of "Object-oriented programming in a .NET and C# environment" course in computer science studies at The Academic College of Tel Aviv-Yaffo - MTA.       

As part of development this project, we implemented two user interfaces for the game - console and windows.                                                             
Implementing different game UI's led us to separate the logical layer and the UI layer. Writing readable, reusable and replacable code combined with seprating parts of code into modules, capsulation and OOP implementation is what guided us during the project.                                                                      
Hope you'll enjoy playing the game (and do not lose)

## Game rules:
Checkers is played by two opponents on opposite sides of the game board. One player has the black pieces the other has the blue pieces. Players alternate turns.         
A player cannot move an opponent's pieces. A move consists of moving a piece diagonally to an adjacent unoccupied square. If the adjacent square contains an opponent's piece, and the square immediately beyond it is vacant, the piece has to captured (and removed from the game) by jumping over it.                                     
Only the beige squares of the checkerboard are used. A piece can only move diagonally into an unoccupied square. When capturing an opponent's piece is possible, capturing is mandatory.                       
#### Number of players:
The player can choose wether to play alone against the computer OR with another player.
#### Board sizes:
There is 3 board sizes which can be set:
- 6x6
- 8x8
- 10x10
#### When does the game end?
The player without pieces remaining, or who cannot move due to being blocked, loses the game.
After finish single game there is an option to start another game and the player's score updating.
#### How is the score calculated?
- Each regular tool remaining at the end - ***1 Point***
- Each king tool remaining at the end - ***4 points***

## Game Demonstration:
![CheckersGameGIF](https://user-images.githubusercontent.com/82831070/174772007-1bbfa615-2760-4d52-bd27-e725eada8e36.gif)

## Other contributors:
[Amir Kirshenzvige](https://github.com/amirkirshen)

