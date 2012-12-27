csharp-play
===========

Group-oriented programming for C#.  
BSD licence.  
For version log, see the individual files.  

##Motivation

A group is a selection of members in a list.  
It can be efficiently computed using intervals.  

    {2, 4, 6, 8}
    
    Contains items from index 2 -> 4.
    Contains items from index 6 -> 8.

This implementation uses operator overloading, so one can join two groups simply by writing:

    var c = a + b
    
If you need the members that are in both groups, you can write:

    var c = a * b
    
If you need the members in the first group but not in the second, write:

    var c = a - b

There are many applications of group, such as filtering, storage pre-computed values and advanced decision making.  
