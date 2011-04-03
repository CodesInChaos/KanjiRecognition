using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace KanjiRecognitionTest
{
	class StrokeMatch
	{
		public readonly ReadOnlyCollection<StrokePartMatch> Parts;
		public readonly float Cost;

		public StrokeMatch(IEnumerable<StrokePartMatch> parts)
		{
			Parts = new ReadOnlyCollection<StrokePartMatch>(parts.ToArray());
			Cost = Parts.Sum(p => p.MinCost);
			//Cost = Parts.Count * (Cost + 0.001f *parts.Sum(p=>p.Delta.LengthSquared));
		}
	}
}
