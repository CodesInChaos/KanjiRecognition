using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Chaos.Util.Mathematics;

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

		public override string ToString()
		{
			if (Parts.Count == 0)
				return "-";
			StringBuilder sb = new StringBuilder();
			foreach (StrokePart part in Parts)
			{
				sb.Append((Vector2i)(part.StartPoint * 100));
				sb.Append(">");
			}
			sb.Append((Vector2i)(Parts.Last().EndPoint * 100));
			return sb.ToString();
		}
	}
}
