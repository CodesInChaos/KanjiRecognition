using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Util.Mathematics;
using System.Globalization;

namespace KanjiRecognitionTest
{
	public class StrokePart
	{
		public float Angle { get { return Delta.AngleTo(); } }
		public Vector2f Delta { get { return EndPoint - StartPoint; } }
		public float Length { get { return Delta.Length; } }
		public readonly float Curve;

		public readonly Vector2f StartPoint;
		public readonly Vector2f EndPoint;
		private readonly Vector2f para;
		private readonly float paraOffset;
		private readonly Vector2f ortho;
		private readonly float orthoOffset;

		public StrokePart(Vector2f startPoint, Vector2f endPoint)
			: this(startPoint, endPoint, 0)
		{
		}

		public StrokePart(Vector2f startPoint, Vector2f endPoint, float curve)
		{
			StartPoint = startPoint;
			EndPoint = endPoint;
			Curve = curve;

			para = 0.5f * Delta / Delta.LengthSquared;
			paraOffset = -StartPoint * para - 1;

			ortho = new Vector2f(Delta.Y, -Delta.X).Normalized;
			orthoOffset = -StartPoint * ortho;
		}

		public float GetParallelPositionScaled(Vector2f globalCoordinate)
		{
			return globalCoordinate * para + paraOffset;
		}

		public float GetOrthoPositionUnscaled(Vector2f globalCoordinate)
		{
			return globalCoordinate * ortho + orthoOffset;
		}

		public Vector2f GetGlobalFromProgress(float progr)
		{
			return GetGlobalFromScaled(2 * progr - 1);
		}

		public Vector2f GetGlobalFromScaled(float scaled)
		{
			float param;
			if (Math.Abs(scaled) < 1)
				param = (1 - scaled * scaled) * Curve;
			else
				param = 0;
			Vector2f result = StartPoint + Delta * (0.5f * (scaled + 1)) + ortho * Length * 0.5f * param;
			return result;
		}

		public override string ToString()
		{
			return ToString(true);
		}

		public string ToString(bool includeEnd)
		{
			string start = (int)(StartPoint.X * 100) + "," + (int)(StartPoint.Y * 100);
			string curve = Curve != 0 ? Curve.ToString(CultureInfo.InvariantCulture) : "";
			string end = (int)(EndPoint.X * 100) + "," + (int)(EndPoint.Y * 100);
			string result = start + ">" + curve + ">";
			if (includeEnd)
				result += end;
			return result;
		}

		public static string StrokeToString(IEnumerable<StrokePart> parts)
		{
			List<StrokePart> partsList = parts.ToList();
			string result = "";
			for (int i = 0; i < partsList.Count; i++)
			{
				StrokePart part = partsList[i];
				bool includeEnd = i == partsList.Count - 1;
				result += part.ToString(includeEnd);
			}
			return result;
		}


	}
}
