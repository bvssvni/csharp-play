/*
History - Group-oriented programming for time.  
BSD license.  
by Sven Nilsen, 2012  
http://www.cutoutpro.com  
Version: 0.001 in angular degrees version notation  
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html  

0.002 - Fixed bug for normal infinite operations.
0.001 - Fixed bug in 'Intersect'.

Redistribution and use in source and binary forms, with or without  
modification, are permitted provided that the following conditions are met:  
1. Redistributions of source code must retain the above copyright notice, this  
list of conditions and the following disclaimer.  
2. Redistributions in binary form must reproduce the above copyright notice,  
this list of conditions and the following disclaimer in the documentation  
and/or other materials provided with the distribution.  
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND  
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED  
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE  
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR  
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES  
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;  
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT  
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS  
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.  
The views and conclusions contained in the software and documentation are those  
of the authors and should not be interpreted as representing official policies,  
either expressed or implied, of the FreeBSD Project.  
*/

using System;
using System.Collections.Generic;

namespace Play
{
	public class History : List<DateTime>
	{
		public bool Inverted = false;

		public History ()
		{
		}

		public static History AllTime ()
		{
			var a = new History ();
			a.Inverted = true;
			return a;
		}

		public History(DateTime[] array) : base(array)
		{
		}

		public static bool IsEmpty(History a)
		{
			if (a == null || a.Count == 0 && !a.Inverted)
				return true;
			
			return false;
		}

		public static bool IsFinite (History a) {
			if (a.Inverted) {
				return false;
			}

			return a.Count % 2 == 0;
		}

		public bool IsSequential () {
			int n = this.Count - 1;
			for (int i = 0; i < n; i++) {
				if (this [i] > this [i+1]) {
					return false;
				}
			}

			return true;
		}

		public static History Union (History a, History b) {
			if (a.Inverted && !b.Inverted) {
				var res = SubSubtract (a, b);
				res.Inverted = true;
				return res;
			}
			if (!a.Inverted && b.Inverted) {
				var res = SubSubtract (b, a);
				res.Inverted = true;
				return res;
			}
			if (a.Inverted && b.Inverted) {
				var res = SubIntersect (a, b);
				res.Inverted = true;
				return res;
			}

			return SubUnion (a, b);
		}

		private static History SubUnion(History a, History b)
		{
			History list = new History();
			
			int a_length = a.Count;
			int b_length = b.Count;
			
			if (a_length == 0 && b_length == 0)
				return list;
			
			if (a_length == 0) {
				list.AddRange(b);
				
				return list;
			}
			if (b_length == 0) {
				list.AddRange(a);
				
				return list;
			}
			
			int i = 0, j = 0; 
			bool isA = false; 
			bool isB = false; 
			bool was = false;
			bool has = false;
			DateTime pa, pb, min;
			while (i < a_length || j < b_length) {
				// Get the least value.
				pa = i >= a_length ? DateTime.MaxValue : a [i];
				pb = j >= b_length ? DateTime.MaxValue : b [j];
				min = pa < pb ? pa : pb;
				
				// Advance the least value, both if both are equal.
				if (pa == min) {
					isA = !isA;
					i++;
				}
				if (pb == min) {
					isB = !isB;
					j++;
				}
				
				// Add to result if this changes the truth value.
				has = isA || isB;
				if (has != was)
					list.Add(min);
				
				was = isA || isB;
			}
			
			return list;
		}

		public static History Intersect (History a, History b) {
			if (a.Inverted && !b.Inverted) {
				return SubSubtract (b, a);
			}
			if (!a.Inverted && b.Inverted) {
				return SubSubtract (a, b);
			}
			if (a.Inverted && b.Inverted) {
				var res = SubUnion (a, b);
				res.Inverted = true;
				return res;
			}

			return SubIntersect (a, b);
		}

		private static History SubIntersect(History a, History b)
		{
			History arr = new History();
			
			int alength = a.Count;
			int blength = b.Count;
			if (alength == 0 || blength == 0)
				return arr;

			int i = 0, j = 0; 
			bool isA = false; 
			bool isB = false; 
			bool was = false;
			bool has = false;
			DateTime pa, pb, min;
			while (i < alength || j < blength) {
				// Get the last value from each group.
				pa = i >= alength ? DateTime.MaxValue : a [i];
				pb = j >= blength ? DateTime.MaxValue : b [j];
				min = pa < pb ? pa : pb;
				
				// Advance the one with least value, both if they got the same.
				if (pa == min) {
					isA = !isA; 
					i++;
				}
				if (pb == min) {
					isB = !isB;
					j++;
				}

				// Find out if the new change should be added to the result.
				has = isA && isB;

				if (has != was)
					arr.Add(min);
				
				was = has;
			}
			
			return arr;
		}

		public static History Subtract (History a, History b) {
			if (a.Inverted && !b.Inverted) {
				var res = SubUnion (a, b);
				res.Inverted = true;
				return res;
			}
			if (!a.Inverted && b.Inverted) {
				return SubIntersect (a, b);
			}
			if (a.Inverted && b.Inverted) {
				return SubSubtract (b, a);
			}

			return SubSubtract (a, b);
		}

		private static History SubSubtract(History a, History b)
		{
			int a_length = a.Count;
			int b_length = b.Count;
			if (b_length == 0) {
				History c = new History(a.ToArray());
				return c;
			}
			
			History arr = new History();
			
			if (a_length == 0 || b_length == 0)
				return arr;

			int i = 0, j = 0; 
			bool isA = false; 
			bool isB = false;
			bool was = false;
			bool has = false;
			DateTime pa, pb, min; 
			while (i < a_length || j < b_length) {
				// Get the last value from each group.
				pa = i >= a_length ? DateTime.MaxValue : a [i];
				pb = j >= b_length ? DateTime.MaxValue : b [j];
				min = pa < pb ? pa : pb;
				
				// Advance the group with least value, both if they are equal.
				if (pa == min) {
					isA = !isA;
					i++;
				}
				if (pb == min) {
					isB = !isB;
					j++;
				}
				
				// If it changes the truth value, add to result.
				has = isA && !isB;
				if (has != was)
					arr.Add(min);
				
				was = has;
			}

			return arr;
		}

		public static History operator +(History a, History b)
		{
			return History.Union(a, b);
		}
		
		public static History operator *(History a, History b)
		{
			return History.Intersect(a, b);
		}
		
		public static History operator -(History a, History b)
		{
			return History.Subtract(a, b);
		}

		public static TimeSpan Sum (History a) {
			if (!History.IsFinite (a)) {
				return TimeSpan.MaxValue;
			}
			
			var sum = new TimeSpan ();
			int n = a.Count >> 1;
			for (int i = 0; i < n; i++) {
				var start = a[i];
				var end = a[i+1];
				sum += end - start;
			}
			
			return sum;
		}

		public History Before (DateTime date) {
			var before = History.AllTime ();
			before.Add (date);
			return this * before;
		}
		
		public History After (DateTime date) {
			var after = new History ();
			after.Add (date);
			return this * after;
		}
		
		public override string ToString()
		{
			var strb = new System.Text.StringBuilder ();
			foreach (var moment in this) {
				strb.Append (moment.ToString () + "\r\n");
			}
			
			return strb.ToString ();
		}
	}
}