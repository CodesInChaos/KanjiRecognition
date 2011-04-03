using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Util.Mathematics;

namespace KanjiRecognitionTest
{
	public class StrokePartMatch:StrokePart
	{
		public float MinCost;
		public float DeltaCurveCost;

		public StrokePartMatch(Vector2f startPoint, Vector2f endPoint, float curve)
			: base(startPoint, endPoint, curve)
		{
		}
	}
}
