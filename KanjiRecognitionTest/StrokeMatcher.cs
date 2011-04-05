using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Util.Mathematics;

namespace KanjiRecognitionTest
{
	class StrokeMatcher
	{
		public static IEnumerable<StrokeMatch> Match(IEnumerable<Vector2f> input)
		{
			StrokeMatcher matcher = new StrokeMatcher(input);
			return matcher.Match();
		}

		private readonly List<Vector2f> Input;
		private readonly List<float> Weight;
		private readonly List<int> InterestingPoints = new List<int>();

		private StrokeMatcher(IEnumerable<Vector2f> input)
		{
			Input = input.ToList();
			if (Input.Count < 2)
				return;
			//CleanInput();
			Weight = Input.Select(v => 0f).ToList();//hack
			for (int i = 1; i < Weight.Count - 1; i++)
			{
				Weight[i] =
					(Input[i - 1] - Input[i]).Length +
					(Input[i + 1] - Input[i]).Length;
			}
			Weight[0] = (Input[1] - Input[0]).Length;
			Weight[Input.Count - 1] = (Input[Input.Count - 1] - Input[Input.Count - 2]).Length;
			float scale = 1 / Weight.Sum();
			for (int i = 0; i < Weight.Count; i++)
				Weight[i] *= scale;

			CalcInterestingPoints();
		}

		private void CalcInterestingPoints()
		{
			CalcInterestingPointsLocal();
		}

		private void CalcInterestingPointsAll()
		{
			InterestingPoints.AddRange(Enumerable.Range(1, Input.Count - 2));
		}

		private void CalcInterestingPointsLocal()
		{
			for (int i = 1; i < Input.Count - 1; i++)
			{
				Vector2f prev = Input[i - 1];
				Vector2f curr = Input[i];
				Vector2f next = Input[i + 1];
				if (curr.X < Math.Min(prev.X, next.X) || curr.X > Math.Max(prev.X, next.X) ||
					curr.Y < Math.Min(prev.Y, next.Y) || curr.Y > Math.Max(prev.Y, next.Y))
				{
					continue;
				}
				Vector2f delta = next - prev;
				Vector2f ortho = new Vector2f(delta.Y, -delta.X) / delta.LengthSquared;
				float dist = (curr - prev) * ortho;
				if (dist > 0.1)
				{
					InterestingPoints.Add(i);
				}
			}
		}

		StrokePartMatch MatchLine(int start, int end)
		{
			StrokePart line = new StrokePart(Input[start], Input[end]);
			float multi = line.Length* 0.5f;

			float a = 0, b = 0, c = 0;//Parabel coefficients
			for (int i = start; i <= end; i++)
			{
				Vector2f point = Input[i];
				float weight = Weight[i];
				float scaledX = line.GetParallelPositionScaled(point);
				float unscaledY = line.GetOrthoPositionUnscaled(point);
				float unscaledOutside = (Math.Abs(scaledX) - 1) * multi;
				if (unscaledOutside > 0)
				{
					c += weight * (unscaledOutside * unscaledOutside + unscaledY  *unscaledY  );
				}
				else
				{
					float param = (1 - scaledX * scaledX) * multi;
					a += weight * param * param;
					b += weight * (-2) * param * unscaledY;
					c += weight * unscaledY * unscaledY;
				}
			}
			float optimalCurve = -b / (2 * a);
			var result = new StrokePartMatch(line.StartPoint, line.EndPoint, optimalCurve);
			result.DeltaCurveCost = a;
			result.MinCost = c - b * b / (4 * a);

			return result;
		}

		private IEnumerable<StrokeMatch> Match()
		{
			if (Input.Count < 2)
				yield break;
			var singleMatch = new List<StrokePartMatch>();
			singleMatch.Add(MatchLine(0, Input.Count - 1));
			yield return new StrokeMatch(singleMatch);

			foreach (int i in InterestingPoints)
			{
				StrokePartMatch match1 = MatchLine(0, i);
				StrokePartMatch match2 = MatchLine(i, Input.Count - 1);
				var match = new List<StrokePartMatch>();
				match.Add(match1);
				match.Add(match2);
				StrokeMatch fullMatch = new StrokeMatch(match);
				yield return fullMatch;
			}
			foreach (int i in InterestingPoints)
				foreach (int j in InterestingPoints)
				{
					if (i >= j)
						continue;
					StrokePartMatch match1 = MatchLine(0, i);
					StrokePartMatch match2 = MatchLine(i, j);
					StrokePartMatch match3 = MatchLine(j, Input.Count - 1);
					var match = new List<StrokePartMatch>();
					match.Add(match1);
					match.Add(match2);
					match.Add(match3);
					StrokeMatch fullMatch = new StrokeMatch(match);
					yield return fullMatch;
				}
		}
	}
}
