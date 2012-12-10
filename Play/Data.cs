using System;
using System.Collections.Generic;

namespace Play
{
	// Data is simply a list of Li constructs.
	public class Data : List<Li>
	{
		public override string ToString ()
		{
			System.Text.StringBuilder strb = new System.Text.StringBuilder();
			foreach (var item in this) 
			{
				strb.Append(item.ToString ());
				strb.Append ("\n");
			}
			return strb.ToString ();
		}
	}
}

