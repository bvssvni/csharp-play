csharp-play
===========

Data structures and useful methods for group-oriented programming. 
BSD licence.  

##Introduction to the concept of a "Group"

A Group is an object that is the same whether any two members swap their location.  
It is very useful in advanced problem solving, such as text parsing or dealing with data.  

In group-oriented programming, one thinks of data, such as arrays or lists,  
as a group which can be partitioned into sub-groups.  
Sub-groups share the same index space and can be composed into new sub-groups.  

The challenge is to find the right proper sub-group for an action.  
Once this is done, one can make algorithms more flexible by using a group as "filter".  
