using System;
using NUnit.Framework;

namespace Play
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void TestIntersect1()
		{
			Group a = new Group();
			a.Add(0);
			a.Add(10);

			Group b = new Group();
			b.Add(5);
			b.Add(15);

			Group c = a * b;
			Assert.AreEqual(2, c.Count);
			Assert.AreEqual(5, c[0]);
			Assert.AreEqual(10, c[1]);
		}

		[Test()]
		public void TestIntersect2()
		{
			Group a = new Group();
			a.Add(0);
			a.Add(10);
			
			Group b = null;
			
			Group c = a * b;
			Assert.AreEqual(true, Group.IsEmpty(c));
		}

		[Test()]
		public void TestUnion1()
		{
			Group a = new Group();
			a.Add(0);
			a.Add(10);

			Group b = new Group();
			b.Add(9);
			b.Add(12);

			Group c = a + b;
			Assert.AreEqual(2, c.Count);
			Assert.AreEqual(0, c[0]);
			Assert.AreEqual(12, c[1]);
		}

		[Test()]
		public void TestUnion2()
		{
			Group a = new Group();
			a.Add(0);
			a.Add(10);
			
			Group b = null;
			
			Group c = a + b;
			Assert.AreEqual(null, c);
		}

		[Test()]
		public void TestSubtract1()
		{
			Group a = new Group();
			a.Add(0);
			a.Add(10);
			
			Group b = new Group();
			b.Add(9);
			b.Add(12);
			
			Group c = a - b;
			Assert.AreEqual(2, c.Count);
			Assert.AreEqual(0, c[0]);
			Assert.AreEqual(9, c[1]);
		}

		[Test()]
		public void TestAdd1()
		{
			Group a = new Group();
			Assert.AreEqual(0, a.Count);
			a += 0;
			Assert.AreEqual(2, a.Count);
			a += 0;
			Assert.AreEqual(1, a[1]);
			a += 1;
			Assert.AreEqual(2, a[1]);
		}

		[Test()]
		public void TestIsEmpty1()
		{
			Group a = new Group();
			Assert.AreEqual(true, Group.IsEmpty(a));
			a += 5;
			Assert.AreEqual(false, Group.IsEmpty(a));
		}

		[Test()]
		public void TestSize()
		{
			Group a = new Group();
			a.Add(0);
			a.Add(10);
			Assert.AreEqual(10, Group.Size(a));
			Assert.True(a > 5);
			Assert.True(a < 12);
			Assert.True(a <= 10);
			Assert.False(a < 10);
			Assert.True(a >= 10);
		}

		[Test()]
		public void TestForward()
		{
			var arr = new int[] {0,1,2,3,4,5,6};
			var g = Group.Slice(2, 4);
			int j = 0;
			foreach (int i in g.Forward<int>(arr))
			{
				Console.WriteLine(i.ToString());
				Assert.True(i == arr[j+2]);
				j++;
			}
		}

		[Test()]
		public void TestBackward()
		{
			var arr = new int[] {0,1,2,3,4,5,6};
			var g = Group.Slice(2, 4);
			int j = 4;
			foreach (int i in g.Backward<int>(arr))
			{
				Console.WriteLine(i.ToString());
				Assert.True(i == arr[j]);
				j--;
			}
		}
	}
}

