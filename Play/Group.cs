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

		public delegate bool IsTrue(Li li);

		// Creates a group by predicate function.
		public static Group Predicate(IsTrue func, Data data)
		{
			Group g = new Group();
			int n = data.Count;
			bool has = false;
			bool was = false;
			for (int i = 0; i < n; i++)
			{
				Li a = data[i];
				if (func(a))
				{
					has = true;
				}
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

		// Creates a group of data intersected by a specific point in time.
		public static Group Time(Data data, long time)
		{
			return Predicate(delegate(Li li) {
				return li.Start <= time && li.End > time;
			}, data);
		}

		// Creates a group of data that has a certain kind.
		public static Group Type(Data data, Type t)
		{
			return Predicate(delegate(Li li) {
				return li.Value.GetType() == t;
			}, data);
		}

		// Adds a new member to a group.
		public static Group Add(Group a, int id)
		{
			Group b = new Group();
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
			bool ba = false; 
			bool bb = false; 
			bool oldB = false;
			int pa, pb;
			while (i < alength || j < blength)
			{
				pa = a[i >= alength ? alength - 1 : i];
				pb = b[j >= blength ? blength - 1 : j];
				
				if (pa == pb)
				{
					ba = !ba;
					bb = !bb;
					if ((ba && bb) != oldB)
						arr.Add(pa);
					
					i++;
					j++;
				}
				else if ((pa < pb || j >= blength) && i < alength)
				{
					ba = !ba;
					if ((ba && bb) != oldB)
						arr.Add(pa);
					
					i++;
				}
				else if (j < blength)
				{
					bb = !bb;
					if ((ba && bb) != oldB)
						arr.Add(pb);
					
					j++;
				}
				else
					break;
				
				oldB = ba && bb;
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
			
			int k;
			if (a_length == 0)
			{
				for (k = 0; k < b_length; k++)
					list.Add(b[k]);
				
				return list;
			}
			if (b_length == 0)
			{
				for (k = 0; k < a_length; k++)
					list.Add(a[k]);
				
				return list;
			}

			int i = 0, j = 0; 
			bool ba = false; 
			bool bb = false; 
			bool oldB = false;
			int pa, pb;
			while (i < a_length || j < b_length)
			{
				pa = a[i >= a_length ? a_length - 1 : i];
				pb = b[j >= b_length ? b_length - 1 : j];
				
				if (pa == pb)
				{
					ba = !ba;
					bb = !bb;
					if ((ba || bb) != oldB)
						list.Add(pa);
					
					i++;
					j++;
				}
				else if ((pa < pb || j >= b_length) && i < a_length)
				{
					ba = !ba;
					if ((ba || bb) != oldB)
						list.Add(pa);
					
					i++;
				}
				else if (j < b_length)
				{
					bb = !bb;
					if ((ba || bb) != oldB)
						list.Add(pb);
					
					j++;
				}
				else
					break;
				
				oldB = ba || bb;
			}
			
			return list;
		}
	
		// Removes all members of _b_ from group _a_.
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
			bool ba = false; 
			bool bb = true; 
			bool oldB = false;
			int pa;
			int pb; 
			while (i < a_length || j < b_length)
			{
				pa = a[i >= a_length ? a_length - 1 : i];
				pb = b[j >= b_length ? b_length - 1 : j];
				
				if (pa == pb)
				{
					ba = !ba;
					bb = !bb;
					if ((ba && bb) != oldB)
						arr.Add(pa);
					
					i++;
					j++;
				}
				else if ((pa < pb || j >= b_length) && i < a_length)
				{
					ba = !ba;
					if ((ba && bb) != oldB)
						arr.Add(pa);
					
					i++;
				}
				else if (j < b_length)
				{
					bb = !bb;
					if ((ba && bb) != oldB)
						arr.Add(pb);
					
					j++;
				}
				else
					break;
				
				oldB = ba && bb;
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
	}

}

