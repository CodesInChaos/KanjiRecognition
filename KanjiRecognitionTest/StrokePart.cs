using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Util.Mathematics;

namespace KanjiRecognitionTest
{
	public abstract class StrokePart
	{
		public float Angle { get { return Delta.AngleTo(); } }
		public Vector2f Delta { get { return EndPoint - StartPoint; } }
		public float Length { get { return Delta.Length; } }
		public readonly float Curve;
		public readonly Vector2f StartPoint;
		public readonly Vector2f EndPoint;

		public StrokePart(Vector2f startPoint, Vector2f endPoint, float curve)
		{
			StartPoint = startPoint;
			EndPoint = endPoint;
			Curve = curve;
		}
	}
}
