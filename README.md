# Solution Basic Information
Author: Bingkun Han
Partner: None
Start Date: 12-Jan-2024
Course: CS 3500, University of Utah, School of Computing
GitHub ID: kogasaa
Repo: https://github.com/uofu-cs3500-spring24/spreadsheet-kogasaa.git
Commit Date: 18-Feb-2024
Solution: Spreadsheet
Copyright: CS 3500 and Bingkun Han - This work may not be copied for use in Academic Coursework.


# Overview of the Spreadsheet functionality

The Spreadsheet making is still on progress, now it has a formula evaluator library to calculate the 
formula and get right answer from the formla. Then It has a dependency to control all relationships 
between deifferent cells. And there is a formula class has more functions as a formula. Next, there is a 
Spreadsheet class to sotre all the cells and dependecies in the spreadsheet. IT can also save and read the 
xml of a apreadsheet. Futrue extensions are still 
in prgress.

# My Software Practices:
	1. Sepration of concerns: it was reflected in the formula project. FOr hte variable validation, there are many situations that this is a bad variable. Also it has
		many rules to create a formula in the formula like balance rules and something. I use helper method with many if statements to consider all the possiable bad 
		situations
	2. Abstraction: this is refelcted in the abstract Spreadsheet. I implemented spreadsheet project really well, so it can achieve all the override method from the 
		abstract methods

	3. regression test: It is the as4 and as5. In as5 I used the as4 tests to make sure the old functionalities work relly well. Then I would not worry more about the 
		old method and start to test new method.


# Time Expenditures:
	
	1. Assignment One:     Predicted Hours:  18 hours            Actual Hours:  22 hours
	2. Assignment Two:     Predicted Hours:  10 hours            Actual Hours:  5 hours(coding) + 1 hour (learning) + 2 hours(debugging)          
	NOTE: Coding is faster than I thought TTD improve my implementation Time

	3. Assignment Three:     Predicted Hours:  14 hours(5h for implementaion + 9h for debugging)            Actual Hours:  3h for writing the tests + 9h for implementaiton and debuging
	Note: it is really near as i expected

	4. Assignment Four:    Predicted Hours: 14 hours             Actual Hours:  6 hours for testing + 5 hours to implementaion
	Note: the inplementaion is longer than I thought, because i was confused about the exception throw in every method

	5. Assignment Four:    Predicted Hours: 112 hours             Actual Hours:  12 hours for testing + 12 hours to implementaion
	Note: The difficulty and complexity of AS5 is out of my mind, expecially for the xml writing and testing. It takes much more time than I thought