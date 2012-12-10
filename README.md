csharp-play
===========

Combining group-oriented programming with time interval data.

##Introduction to the concept of a "Li"

A "Li" is a piece of information that has a starting time and an end time.
In some ways it is the ur-structure of data, a step before it becomes a pure data model.
The word "li" is a short form for "line", but spans over time instead of space.

It is possible to construct any relations with li structures.
A li structure can also be a complex one, but it is specially powerful in simple form.
To understand how it works it requires some imagination.

Let us pretend we are building a drawing application.
In standard programming, we would connect the stroke the user does with a brush.
The user might wish to change the brush for a new stroke, or replace the existing brush with another.

When using a li structure, we simply record the settings in the palette and the canvas separately.
In the drawing routine, we look up settings that were active at the same moment as stroke was created.
Since the li structure of the palette spans over the stroke, it will be recreated with the same brush.
When the user replaces the brush with another, we change the "history" and therefore only strokes
that had the same setting.

The simpler structure, the more independent changes can we make to the history.
We also record the changes of history as li structures, so the user can undo/redo the actions.
There are several techniques of doing this, but if it gets too complicated one can just ignore
the start/end interval and use the data itself.

##Introduction of the concept of a "Group"

A "Group" is an ordered selection of identifier numbers.
It can be the positions of characters in some text or the indices in a list.

The format "group bitstream" is storing two numbers per slice of indices that fulfill the same condition.
This format is slower when inserting or deleting a single item, but faster for whole groups.
Because a group satisfy Boolean algebra (union, intersection, subtraction) it can be thought of a "superbit".

Group oriented programming is a powerful concept where you deduce information by creating and reusing groups.
The code tend to be much simpler and clearer than traditional programming.
There are less loops because you perform conditions on the groups as a whole, not on single items.

A function that returns True or False is called a predicate.
In predicate logic, it is common to say "for all"  and "exists".

"For all" means a condition is satisfied for all items in a group.
Written in group oriented programming, this becomes

    f(a) == a
    
"Exists" means there is at least one item that fulfill the condition in a group.
Written in group oriented programming, this becomes

    f(a) > 0
    
