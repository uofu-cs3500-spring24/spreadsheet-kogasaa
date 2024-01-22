# Project Basic Information
Author: Bingkun Han
Partner: None
Start Date: 22-Jan-2024
Course: CS 3500, University of Utah, School of Computing
GitHub ID: kogasaa
Repo: https://github.com/uofu-cs3500-spring24/spreadsheet-kogasaa.git
Commit Date: 22-Jan-2024
Project: DependencyGraph
Copyright: CS 3500 and Bingkun Han - This work may not be copied for use in Academic Coursework.

# Comments to Evaluators
This is the implementation of the DepeandencyGraph API. The data structure I chose is the hashset and tuple. I use hashset to 
store all the dependency relationships in the graph. 

Tuple is to store a dependee and a dependent in a relationship. For example, ("A1", "A2") is a relation presented by tuple. The
first item - A1, is dependee of the second item - A2.

HashSet is to store the relationships. It contains the specific tuples without duplication.

The reason I choose tuple is because I think tuple is really easy to understand, and it has simple structure so the complixty is also low.
Why I choose HashSet is because HashSet does not allow the duplicated relations exists and has low complexity.


# Assignment Specific Topics
Implementation of a API.
Test Driven Development.

# Consulted Peers
None. I read all the instructions of the assignment, and I think it is not difficult so I did all by myself

# Referencing
CS3500 Assignment Two Duide - https://docs.google.com/document/d/1eB3YWaXpMuaRg4c28fJFNwlyZwzib10twioAJxu0z0A/edit?usp=sharing
Microsoft Learning - https://learn.microsoft.com/en-us/visualstudio/get-started/visual-studio-ide?view=vs-2022 (I study the gen-
reic stack class from the Microsoft Learning)