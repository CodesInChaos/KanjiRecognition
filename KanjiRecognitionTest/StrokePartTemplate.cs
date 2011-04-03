using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Util.Mathematics;

namespace KanjiRecognitionTest
{
	public class StrokePartTemplate : StrokePart
	{
		public StrokePartTemplate(Vector2f startPoint, Vector2f endPoint, float curve)
			: base(startPoint, endPoint, curve)
		{
		}

		public float MatchCost(StrokePartMatch match)
		{
			float lenDelta = match.Delta.Length - Length;
			float lenDeltaSquared = lenDelta * lenDelta;
			float angleDelta = (match.Delta.AngleTo() - Angle) / (2 * (float)Math.PI);
			angleDelta = 2 * (angleDelta - (float)Math.Floor(angleDelta + 0.5f));
			float angleDeltaSquared = angleDelta * angleDelta;
			float curveDelta = match.Curve - Curve;
			float curveDeltaSquared = curveDelta * curveDelta;
			float startDeltaSquared = (match.StartPoint - StartPoint).LengthSquared;
			float endDeltaSquared = (match.StartPoint - StartPoint).LengthSquared;
			float cost = 4*lenDeltaSquared + 
				5*(startDeltaSquared + endDeltaSquared)
				+ curveDeltaSquared + 8*angleDeltaSquared;
			return cost;
		}
	}
}
