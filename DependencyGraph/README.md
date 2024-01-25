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
This is the implementation of the DepeandencyGraph API. The data structures I chose are dictionary and hashset. I create two 
Dictionaries - dee_dentGroup and dent_deeGroup. dee_dent means dependee is the key, hashset of dependents is value; dent_dee 
means dependent is the value, hash set of dependees is value

For example if I want to add relation - ("A1", "B2"), ("A1", "C2"), ("B2", "C2")
dee_dent: "A1":{"B2", "C2"}; "B2":{"C2"}
dent_dee: "C2":{"A1", "C2"}; "B2":{"A1"}

The reason I chose Dictionary is because dictionary would not allow duplicated key exists, Same as the reason I choose HashSet
which also prevent duplicated values exist.



# Assignment Specific Topics
Implementation of a API.
Test Driven Development.

# Consulted Peers
None. I read all the instructions of the assignment, and I think it is not difficult so I did all by myself

# Referencing
CS3500 Assignment Two Duide - https://utah.instructure.com/courses/940768/assignments/13804648
Microsoft Learning - https://learn.microsoft.com/en-us/visualstudio/get-started/visual-studio-ide?view=vs-2022 (I study the HashSet<T> and Tuple
classes from the Microsoft Learning)