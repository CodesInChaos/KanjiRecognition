using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaos.Util.Mathematics;

namespace KanjiRecognitionTest
{
	class KanjiMatcher
	{
		List<List<StrokeMatch>> InputMatches = new List<List<StrokeMatch>>();
		List<List<Vector2f>> Input;
		List<StrokeTemplate> Template;
		List<StrokeMatch> BestMatch;

		public static float Match(List<List<Vector2f>> input, List<StrokeTemplate> template, out List<StrokeMatch> match)
		{
			match = null;
			if (input.Count != template.Count)
				return float.PositiveInfinity;
			KanjiMatcher matcher = new KanjiMatcher();
			matcher.Input = input.Select(stroke => stroke.ToList()).ToList();//Deep copy input
			matcher.Template = template;
			matcher.Resize();
			matcher.MatchStrokes();			
			float cost=matcher.TotalCost();
			match = matcher.BestMatch;
			return cost;
		}

		void Resize()
		{
			IEnumerable<Vector2f> allPoints = Input.SelectMany(stroke => stroke);
			float x1 = allPoints.Min(v => v.X);
			float x2 = allPoints.Max(v => v.X);
			float y1 = allPoints.Min(v => v.Y);
			float y2 = allPoints.Max(v => v.Y);
			var templateParts = Template.SelectMany(stroke => stroke.Parts);
			float templateWidth = Math.Max(templateParts.Max(part => part.StartPoint.X), templateParts.Max(part => part.EndPoint.X));
			float templateHeight = Math.Max(templateParts.Max(part => part.StartPoint.Y), templateParts.Max(part => part.EndPoint.Y));
			float scaleX = 1 / (x2 - x1)*Math.Max(0.2f,templateWidth);
			float scaleY = 1 / (y2 - y1)*Math.Max(0.2f,templateHeight);
			if (scaleX > 1.5f * scaleY)
				scaleX = 1.5f * scaleY;
			if (scaleY > 1.5f * scaleX)
				scaleY = 1.5f * scaleX;
			foreach (var inputStroke in Input)
			{
				for (int i = 0; i < inputStroke.Count; i++)
				{
					inputStroke[i] = (inputStroke[i] - new Vector2f(x1, y1));
					inputStroke[i] = new Vector2f(inputStroke[i].X * scaleX, inputStroke[i].Y * scaleY);
				}
			}
		}

		void MatchStrokes()
		{
			foreach (List<Vector2f> inputStroke in Input)
			{
				InputMatches.Add(StrokeMatcher.Match(inputStroke).ToList());
			}
		}

		float TotalCost()
		{
			float cost = 0;
			BestMatch = new List<StrokeMatch>();
			for (int i = 0; i < Template.Count; i++)
			{
				StrokeMatch strokeMatch;
				cost += StrokeCost(i, out strokeMatch);
				BestMatch.Add(strokeMatch);
			}
			return cost;
		}

		float StrokeCost(int strokeIndex, out StrokeMatch bestMatch)
		{
			StrokeTemplate template = Template[strokeIndex];
			List<StrokeMatch> candidateMatches = InputMatches[strokeIndex];
			float bestCost = float.PositiveInfinity;
			bestMatch = null;
			foreach (StrokeMatch match in candidateMatches)
			{
				if (match.Parts.Count != template.Parts.Count)
					continue;
				float cost = 0;
				for (int partIndex = 0; partIndex < match.Parts.Count; partIndex++)
				{
					StrokePartMatch partMatch = match.Parts[partIndex];
					StrokePartTemplate partTemplate = template.Parts[partIndex];
					cost += partTemplate.MatchCost(partMatch);
				}
				cost += match.Cost*100;
				if (cost < bestCost)
				{
					bestCost = cost;
					bestMatch = match;
				}
			}
			return bestCost;
		}
	}
}
