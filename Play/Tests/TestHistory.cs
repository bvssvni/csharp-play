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

		[Test()]
		public void TestUnion ()
		{
			var a = new History ();
			a.Add (new DateTime (2005, 1, 1));
			a.Add (new DateTime (2005, 1, 6));

			var b = new History ();
			b.Add (new DateTime (2005, 1, 5));
			b.Add (new DateTime (2005, 1, 10));

			var c = History.Union (a, b);
			Assert.True (c.Count == 2);
			Assert.True (c[0] == new DateTime (2005, 1, 1));
			Assert.True (c[1] == new DateTime (2005, 1, 10));
		}

		[Test()]
		public void TestIntersect ()
		{
			var a = new History ();
			a.Add (new DateTime (2005, 1, 1));
			a.Add (new DateTime (2005, 1, 6));
			
			var b = new History ();
			b.Add (new DateTime (2005, 1, 5));
			b.Add (new DateTime (2005, 1, 10));

			var c = History.Intersect (a, b);
			Assert.True (c.Count == 2);
			Assert.True (c[0] == new DateTime (2005, 1, 5));
			Assert.True (c[1] == new DateTime (2005, 1, 6));
		}

		[Test()]
		public void TestSubtract ()
		{
			var a = new History ();
			a.Add (new DateTime (2005, 1, 1));
			a.Add (new DateTime (2005, 1, 6));
			
			var b = new History ();
			b.Add (new DateTime (2005, 1, 5));
			b.Add (new DateTime (2005, 1, 10));
			
			var c = History.Subtract (a, b);
			Assert.True (c.Count == 2);
			Assert.True (c[0] == new DateTime (2005, 1, 1));
			Assert.True (c[1] == new DateTime (2005, 1, 5));
		}

		[Test()]
		public void TestIsEmpty () 
		{
			var a = new History ();
			Assert.True (History.IsEmpty (a));
			Assert.True (History.IsEmpty (null));
			a.Add (new DateTime (2004, 1, 1));
			Assert.False (History.IsEmpty (a));
		}
	}
}

