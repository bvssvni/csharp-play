using System;
using System.Collections.Generic;

namespace Play
{
	// A group is a object loose coupled with data that precalculates a truth value about some information.
	// It is both a way to extract information out of data and a way to access data faster than by looking for it.
	public class Group : List<int>, IComparable
	{
		public Group()
		{

		}

		public Group(int[] array) : base(array)
		{
		}

		public delegate bool IsTrue<T>(T item);

		/// <summary>
		/// A predicate is a function that returns true or false.
		/// This method constructs a group from feeding a function one by one with data.
		/// The indices in the list of data is used to set the group.
		/// </summary>
		/// <param name='func'>
		/// A predicate function.
		/// </param>
		/// <param name='data'>
		/// The data to check for condition.
		/// </param>
		/// <typeparam name='T'>
		/// The type of data in the list.
		/// </typeparam>
		public static Group Predicate<T>(IsTrue<T> func, IList<T> data)
		{
			Group g = new Group();
			int n = data.Count;
			bool has = false;
			bool was = false;
			for (int i = 0; i < n; i++)
			{
				var a = data[i];
				has = func(a);
				if (has != was)
				{
					g.Add(i);
				}
				was = has;
			}
			if (was)
			{
				g.Add(n);
			}
			return g;
		}

		/// <summary>
		/// Adds a member to a group.
		/// </summary>
		/// <param name='a'>
		/// The group to add the member to.
		/// </param>
		/// <param name='id'>
		/// The id of the new member.
		/// </param>
		public static Group Add(Group a, int id)
		{
			var b = new Group();
			b.Add(id);
			b.Add(id + 1);
			return a + b;
		}

		public static Group operator +(Group a, int id)
		{
			return Group.Add(a, id);
		}

		// Returns the number of members in the group.
		public static int Size(Group a)
		{
			int size = 0;
			int na = a.Count / 2;
			for (int i = 0; i < na; i++)
			{
				size += a[i * 2 + 1] - a[i * 2];
			}
			return size;
		}

		public int CompareTo(object obj)
		{
			int size = Group.Size(this);

			return size.CompareTo(obj);
		}

		public static bool operator <(Group a, int n)
		{
			return a.CompareTo(n) < 0;
		}

		public static bool operator >(Group a, int n)
		{
			return a.CompareTo(n) > 0;
		}

		public static bool operator <=(Group a, int n)
		{
			return a.CompareTo(n) <= 0;
		}

		public static bool operator >=(Group a, int n)
		{
			return a.CompareTo(n) >= 0;
		}

		// Returns true if the group is empty, which includes null.
		public static bool IsEmpty(Group a)
		{
			if (a == null || a.Count == 0)
				return true;

			return false;
		}

		// Intersects two groups and creates a new one.
		public static Group Intersect(Group a, Group b)
		{
			if (a == null || b == null)
				return null;

			Group arr = new Group();

			int alength = a.Count;
			int blength = b.Count;
			if (alength == 0 || blength == 0)
				return arr;

			int i = 0, j = 0; 
			bool isA = false; 
			bool isB = false; 
			bool was = false;
			bool has = false;
			int pa, pb, min;
			while (i < alength && j < blength)
			{
				// Get the last value from each group.
				pa = i >= alength ? int.MaxValue : a[i];
				pb = j >= blength ? int.MaxValue : b[j];
				min = pa < pb ? pa : pb;

				// Advance the one with least value, both if they got the same.
				if (pa == min) 
				{
					isA = !isA; 
					i++;
				}
				if (pb == min)
				{
					isB = !isB;
					j++;
				}

				// Find out if the new change should be added to the result.
				has = isA && isB;
				if (has != was) arr.Add(min);

				was = has;
			}
			
			return arr;
		}

		// Creates a group that contains member of both groups.
		public static Group Union(Group a, Group b)
		{
			if (a == null || b == null)
				return null;

			Group list = new Group();

			int a_length = a.Count;
			int b_length = b.Count;

			if (a_length == 0 && b_length == 0)
				return list;

			if (a_length == 0)
			{
				list.AddRange(b);
				
				return list;
			}
			if (b_length == 0)
			{
				list.AddRange(a);
				
				return list;
			}

			int i = 0, j = 0; 
			bool isA = false; 
			bool isB = false; 
			bool was = false;
			bool has = false;
			int pa, pb, min;
			while (i < a_length || j < b_length)
			{
				// Get the least value.
				pa = i >= a_length ? int.MaxValue : a[i];
				pb = j >= b_length ? int.MaxValue : b[j];
				min = pa < pb ? pa : pb;

				// Advance the least value, both if both are equal.
				if (pa == min)
				{
					isA = !isA;
					i++;
				}
				if (pb == min)
				{
					isB = !isB;
					j++;
				}

				// Add to result if this changes the truth value.
				has = isA || isB;
				if (has != was) list.Add(min);
				
				was = isA || isB;
			}
			
			return list;
		}
	
		// Removes all members of _b_ from group _a_.
		/// <summary>
		/// Creates a group that contains member of 'a' but not 'b'.
		/// </summary>
		/// <param name='a'>
		/// The group with the members to choose from.
		/// </param>
		/// <param name='b'>
		/// The group that we should not have members from.
		/// </param>
		public static Group Subtract(Group a, Group b)
		{
			if (a == null || b == null)
				return null;

			int a_length = a.Count;
			int b_length = b.Count;
			if (b_length == 0)
			{
				Group c = new Group(a.ToArray());
				return c;
			}

			Group arr = new Group();
			
			if (a_length == 0 || b_length == 0)
				return arr;
			
			int i = 0, j = 0; 
			bool isA = false; 
			bool isB = false;
			bool was = false;
			bool has = false;
			int pa, pb, min; 
			while (i < a_length)
			{
				// Get the last value from each group.
				pa = i >= a_length ? int.MaxValue : a[i];
				pb = j >= b_length ? int.MaxValue : b[j];
				min = pa < pb ? pa : pb;

				// Advance the group with least value, both if they are equal.
				if (pa == min)
				{
					isA = !isA;
					i++;
				}
				if (pb == min)
				{
					isB = !isB;
					j++;
				}

				// If it changes the truth value, add to result.
				has = isA && !isB;
				if (has != was) arr.Add(min);

				was = has;
			}
			
			return arr;
		}

		public static Group operator +(Group a, Group b)
		{
			return Group.Union(a, b);
		}

		public static Group operator *(Group a, Group b)
		{
			return Group.Intersect(a, b);
		}

		public static Group operator -(Group a, Group b)
		{
			return Group.Subtract(a, b);
		}

		public static bool AreEqual(Group a, Group b)
		{
			bool aEmpty = Group.IsEmpty(a);
			bool bEmpty = Group.IsEmpty(b);
			if (aEmpty && bEmpty)
				return true;
			if (aEmpty != bEmpty)
				return false;

			int na = a.Count;
			int nb = b.Count;
			if (na != nb)
				return false;

			for (int i = 0; i < na; i++)
			{
				if (a[i] != b[i])
					return false;
			}

			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj is Group)
			{
				return Group.AreEqual(this, obj as Group);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Returns a group that contains the indices from 'start' including 'end'.
		/// </summary>
		/// <param name='start'>
		/// The first index.
		/// </param>
		/// <param name='end'>
		/// The last index.
		/// </param>
		public static Group Slice(int start, int end)
		{
			var g = new Group(new int[]{start, end+1});
			return g;
		}

		/// <summary>
		/// Returns an enumerator iterating forward a list.
		/// </summary>
		/// <param name='list'>
		/// The list to iterate through.
		/// </param>
		/// <typeparam name='T'>
		/// The type of object to return.
		/// </typeparam>
		public IEnumerable<T> Forward<T>(IList<T> list)
		{
			int n = this.Count/2;
			for (int i = 0; i < n; i++)
			{
				int start = this[i*2];
				int end = this[i*2+1];
				for (int j = start; j < end; j++)
				{
					yield return list[j];
				}
			}
		}

		/// <summary>
		/// Returns an enumerator iterating backward a list.
		/// </summary>
		/// <param name='list'>
		/// The list to iterate through.
		/// </param>
		/// <typeparam name='T'>
		/// The type of object to return.
		/// </typeparam>
		public IEnumerable<T> Backward<T>(IList<T> list)
		{
			int n = this.Count/2;
			for (int i = n-1; i >= 0; i--)
			{
				int start = this[i*2];
				int end = this[i*2+1];
				for (int j = end-1; j >= start; j--)
				{
					yield return list[j];
				}
			}
		}
	}

}

