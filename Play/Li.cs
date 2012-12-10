using System;

namespace Play
{
	
	// A Li is the basic structure for information.
	// All information has a start time and end time.
	public class Li
	{
		public long Start;
		public long End;
		public object Value;
		
		public Li()
		{
			
		}

		public Li(long start, long end)
		{
			this.Start = start;
			this.End = end;
		}
		
		public Li(Data data)
		{
			data.Add (this);
		}

		public void StartWatch()
		{
			this.Start = DateTime.Now.ToFileTimeUtc();
			this.End = long.MaxValue;
		}
		
		public void StopWatch()
		{
			this.End = DateTime.Now.ToFileTimeUtc();
		}
		
		public override string ToString ()
		{
			return Start.ToString () + "," + End.ToString () + "," + Value.ToString (); 
		}
		
	}

}

