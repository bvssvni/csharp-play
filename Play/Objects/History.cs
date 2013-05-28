/*
History - Group-oriented programming for time.  
BSD license.  
by Sven Nilsen, 2012  
http://www.cutoutpro.com  
Version: 0.000 in angular degrees version notation  
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html  

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
		public bool IsFinite () {
			return this.Count % 2 == 0;
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

		public static History Union(History a, History b)
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

		public History()
		{
		}
	}
}

