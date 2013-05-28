using System;
using NUnit.Framework;

namespace Play
{
	[TestFixture()]
	public class TestHistory
	{
		[Test()]
		public void TestIsFinite()
		{
			var history = new History ();
			history.Add (new DateTime (2002, 1, 1));
			Assert.False (history.IsFinite ());
			history.Add (new DateTime (2002, 1, 4));
			Assert.True (history.IsFinite ());
		}

		[Test()]
		public void TestIsSequential ()
		{
			var a = new History ();
			a.Add (new DateTime (2003, 1, 1));
			a.Add (new DateTime (2002, 12, 18));
			Assert.False (a.IsSequential ());

			var b = new History ();
			b.Add (new DateTime (2002, 12, 18));
			b.Add (new DateTime (2003, 1, 1));
			Assert.True (b.IsSequential ());
		}
	}
}

