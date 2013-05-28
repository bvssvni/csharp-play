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
			Assert.False (History.IsFinite (history));
			history.Add (new DateTime (2002, 1, 4));
			Assert.True (History.IsFinite (history));
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

			var c = a + b;
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

			var c = a * b;
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
			
			var c = a - b;
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

		[Test()]
		public void TestAllTimeIsNotEmpty ()
		{
			var a = History.AllTime ();
			Assert.False (History.IsEmpty (a));
		}

		[Test()]
		public void TestAllTimeIsNotFinite ()
		{
			var a = History.AllTime ();
			Assert.False (History.IsFinite (a));
		}

		[Test()]
		public void TestAllTime ()
		{
			var a = History.AllTime ();
			Assert.True (a.Count == 0);
			Assert.True (a.Inverted);
		}

		[Test()]
		public void TestInvertedUnion ()
		{
			var a = History.AllTime ();
			a.Add (new DateTime (2010, 1, 1));
			a.Add (new DateTime (2010, 1, 10));

			var b = new History ();
			b.Add (new DateTime (2010, 1, 8));
			b.Add (new DateTime (2010, 1, 12));

			var c = History.Union (a, b);
			Assert.True (c.Inverted);
			Assert.True (c[0] == new DateTime (2010, 1, 1));
			Assert.True (c[1] == new DateTime (2010, 1, 8));

			var d = History.Union (b, a);
			Assert.True (d.Inverted);
			Assert.True (d[0] == new DateTime (2010, 1, 1));
			Assert.True (d[1] == new DateTime (2010, 1, 8));

			b.Inverted = true;
			var e = History.Union (a, b);
			Assert.True (e.Inverted);
			Assert.True (e[0] == new DateTime (2010, 1, 8));
			Assert.True (e[1] == new DateTime (2010, 1, 10));
		}

		[Test()]
		public void TestNormalInfiniteUnion ()
		{
			var a = new History ();
			a.Add (new DateTime (2003, 1, 1));

			var b = new History ();
			b.Add (new DateTime (2003, 1, 10));
			b.Add (new DateTime (2003, 1, 12));

			var c = a + b;
			Assert.True (c.Count == 1);
			Assert.True (c[0] == new DateTime (2003, 1, 1));

			var d = b + a;
			Assert.True (d.Count == 1);
			Assert.True (d[0] == new DateTime (2003, 1, 1));
		}

		[Test()]
		public void TestNormalInfiniteUnion2 ()
		{
			var a = new History ();
			a.Add (new DateTime (2009, 1, 1));

			var b = new History ();
			b.Add (new DateTime (2009, 1, 2));

			var c = a + b;
			Assert.True (c.Count == 1);
			Assert.True (c[0] == new DateTime (2009, 1, 1));

			var d = b + a;
			Assert.True (d.Count == 1);
			Assert.True (d[0] == new DateTime (2009, 1, 1));
		}

		[Test()]
		public void TestInvertedIntersect ()
		{
			var a = History.AllTime ();
			a.Add (new DateTime (2010, 1, 1));
			a.Add (new DateTime (2010, 1, 10));
			
			var b = new History ();
			b.Add (new DateTime (2010, 1, 8));
			b.Add (new DateTime (2010, 1, 12));
			
			var c = History.Intersect (a, b);
			Assert.False (c.Inverted);
			Assert.True (c[0] == new DateTime (2010, 1, 1));
			Assert.True (c[1] == new DateTime (2010, 1, 8));

			var d = History.Intersect (b, a);
			Assert.False (d.Inverted);
			Assert.True (d[0] == new DateTime (2010, 1, 1));
			Assert.True (d[1] == new DateTime (2010, 1, 8));

			b.Inverted = true;
			var e = History.Intersect (a, b);
			Assert.True (e.Inverted);
			Assert.True (e[0] == new DateTime (2010, 1, 1));
			Assert.True (e[1] == new DateTime (2010, 1, 12));
		}

		[Test()]
		public void TestNormalInfiniteIntersect ()
		{
			var a = new History ();
			a.Add (new DateTime (2009, 1, 1));

			var b = new History ();
			b.Add (new DateTime (2010, 1, 1));
			b.Add (new DateTime (2010, 1, 10));

			var c = a * b;
			Assert.False (c.Inverted);
			Assert.True (c.Count == 2);
			Assert.True (c[0] == new DateTime (2010, 1, 1));
			Assert.True (c[1] == new DateTime (2010, 1, 10));
		}

		[Test()]
		public void TestNormalInfiniteIntersect2 ()
		{
			var a = new History ();
			a.Add (new DateTime (2002, 1, 1));

			var b = new History ();
			b.Add (new DateTime (2002, 1, 10));

			var c = a * b;
			Assert.True (c.Count == 1);
			Assert.True (c[0] == new DateTime (2002, 1, 10));

			var d = b * a;
			Assert.True (d.Count == 1);
			Assert.True (d[0] == new DateTime (2002, 1, 10));
		}

		[Test()]
		public void TestInvertedSubtract ()
		{
			var a = History.AllTime ();
			a.Add (new DateTime (2010, 1, 1));
			a.Add (new DateTime (2010, 1, 10));
			
			var b = new History ();
			b.Add (new DateTime (2010, 1, 8));
			b.Add (new DateTime (2010, 1, 12));
			
			var c = History.Subtract (a, b);
			Assert.True (c.Inverted);
			Assert.True (c[0] == new DateTime (2010, 1, 1));
			Assert.True (c[1] == new DateTime (2010, 1, 12));

			var d = History.Subtract (b, a);
			Assert.False (d.Inverted);
			Assert.True (d[0] == new DateTime (2010, 1, 8));
			Assert.True (d[1] == new DateTime (2010, 1, 10));

			b.Inverted = true;
			var e = History.Subtract (a, b);
			Assert.False (e.Inverted);
			Assert.True (e[0] == new DateTime (2010, 1, 10));
			Assert.True (e[1] == new DateTime (2010, 1, 12));

			var f = History.Subtract (b, a);
			Assert.False (f.Inverted);
			Assert.True (f[0] == new DateTime (2010, 1, 1));
			Assert.True (f[1] == new DateTime (2010, 1, 8));
		}

		[Test()]
		public void TestNormalInfiniteSubtract ()
		{
			var a = new History ();
			a.Add (new DateTime (2004, 1, 1));

			var b = new History ();
			b.Add (new DateTime (2005, 1, 1));
			b.Add (new DateTime (2005, 1, 4));

			var c = a - b;
			Assert.True (c.Count == 3);
			Assert.True (c[0] == new DateTime (2004, 1, 1));
			Assert.True (c[1] == new DateTime (2005, 1, 1));
			Assert.True (c[2] == new DateTime (2005, 1, 4));

			var d = b - a;
			Assert.True (d.Count == 0);
		}

		[Test()]
		public void TestNormalInfiniteSubtract2 ()
		{
			var a = new History ();
			a.Add (new DateTime (2003, 1, 1));

			var b = new History ();
			b.Add (new DateTime (2003, 1, 5));

			var c = a - b;
			Assert.True (c.Count == 2);
			Assert.True (c[0] == new DateTime (2003, 1, 1));
			Assert.True (c[1] == new DateTime (2003, 1, 5));

			var d = b - a;
			Assert.True (d.Count == 0);
		}

		[Test()]
		public void TestSum ()
		{
			var a = new History ();
			a.Add (new DateTime (2008, 3, 1));
			a.Add (new DateTime (2008, 3, 8));
			var sum = History.Sum (a);
			Assert.True (sum.TotalDays == 7);
		}
	}
}

