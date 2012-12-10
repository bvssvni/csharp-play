using System;
using NUnit.Framework;

namespace Play
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void TestGroup1 ()
		{
			Data data = new Data();
			Li a = new Li();
			a.Start = 0;
			a.End = 10;
			data.Add (a);

			Group g = Group.Time (data, 3);
			Assert.AreEqual (2, g.Count);
			Assert.AreEqual (0, g[0]);
			Assert.AreEqual (1, g[1]);
		}

		[Test()]
		public void TestGroup2()
		{
			Data data = new Data();
			Li a = new Li();
			a.Start = 0;
			a.End = 10;
			data.Add (a);

			Li b = new Li();
			b.Start = 5;
			b.End = 15;
			data.Add (b);

			Group g = Group.Time (data, 6);
			Assert.AreEqual (2, g.Count);
			Assert.AreEqual (0, g[0]);
			Assert.AreEqual (2, g[1]);
		}

		[Test()]
		public void TestType1()
		{
			Data data = new Data();
			Li a = new Li(data);
			a.Value = 20.0;

			Group g = Group.Type (data, typeof(double));
			Assert.AreEqual(2, g.Count);
			Assert.AreEqual (0, g[0]);
			Assert.AreEqual (1, g[1]);
		}

		[Test()]
		public void TestType2()
		{
			Data data = new Data();
			var a = new Li(data);
			a.Value = 20.0;
			var b = new Li(data);
			b.Value = "Sven";

			Group g = Group.Type (data, typeof(string));
			Assert.AreEqual (2, g.Count);
			Assert.AreEqual (1, g[0]);
			Assert.AreEqual (2, g[1]);
		}

		[Test()]
		public void TestIntersect1()
		{
			Group a = new Group();
			a.Add (0);
			a.Add (10);

			Group b = new Group();
			b.Add (5);
			b.Add (15);

			Group c = a * b;
			Assert.AreEqual (2, c.Count);
			Assert.AreEqual (5, c[0]);
			Assert.AreEqual(10, c[1]);
		}

		[Test()]
		public void TestIntersect2()
		{
			Group a = new Group();
			a.Add (0);
			a.Add (10);
			
			Group b = null;
			
			Group c = a * b;
			Assert.AreEqual (true, Group.IsEmpty(c));
		}

		[Test()]
		public void TestUnion1()
		{
			Group a = new Group();
			a.Add (0);
			a.Add (10);

			Group b = new Group();
			b.Add (9);
			b.Add (12);

			Group c = a + b;
			Assert.AreEqual (2, c.Count);
			Assert.AreEqual(0, c[0]);
			Assert.AreEqual (12, c[1]);
		}

		[Test()]
		public void TestUnion2()
		{
			Group a = new Group();
			a.Add (0);
			a.Add (10);
			
			Group b = null;
			
			Group c = a + b;
			Assert.AreEqual (null, c);
		}

		[Test()]
		public void TestSubtract1()
		{
			Group a = new Group();
			a.Add (0);
			a.Add (10);
			
			Group b = new Group();
			b.Add (9);
			b.Add (12);
			
			Group c = a-b;
			Assert.AreEqual (2, c.Count);
			Assert.AreEqual(0, c[0]);
			Assert.AreEqual (9, c[1]);
		}

		[Test()]
		public void TestAdd1()
		{
			Group a = new Group();
			Assert.AreEqual (0, a.Count);
			a += 0;
			Assert.AreEqual (2, a.Count);
			a += 0;
			Assert.AreEqual (1, a[1]);
			a += 1;
			Assert.AreEqual (2, a[1]);
		}

		[Test()]
		public void TestIsEmpty1()
		{
			Group a = new Group();
			Assert.AreEqual (true, Group.IsEmpty(a));
			a += 5;
			Assert.AreEqual (false, Group.IsEmpty (a));
		}
	}
}

