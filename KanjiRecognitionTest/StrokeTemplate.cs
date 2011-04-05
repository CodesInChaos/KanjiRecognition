using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Chaos.Util.Mathematics;
using Chaos.Util.TreeDocuments;
using System.Globalization;

namespace KanjiRecognitionTest
{
	class StrokeTemplate
	{
		public readonly ReadOnlyCollection<StrokePartTemplate> Parts;


		private static Vector2f ParseVector(string s)
		{
			string[] parts = s.Split(',');
			if (parts.Length != 2)
				throw new ArgumentException("Invalid vector " + s);
			float x = float.Parse(parts[0], CultureInfo.InvariantCulture);
			float y = float.Parse(parts[1], CultureInfo.InvariantCulture);
			return new Vector2f(x, y);
		}

		public static StrokeTemplate Parse(string s)
		{
			string[] strParts = s.Split('>');
			if (strParts.Length % 2 == 0)
				throw new ArgumentException("Invalid Stroke " + s);
			List<StrokePartTemplate> parts = new List<StrokePartTemplate>();
			for (int i = 0; i < strParts.Length / 2; i++)
			{
				string strStart = strParts[2 * i];
				string strCurve = strParts[2 * i + 1];
				string strEnd = strParts[2 * i + 2];
				float curve = strCurve == "" ? 0 : float.Parse(strCurve, CultureInfo.InvariantCulture);
				Vector2f start = ParseVector(strStart) / 100;
				Vector2f end = ParseVector(strEnd) / 100;
				StrokePartTemplate part = new StrokePartTemplate(start, end, curve);
				parts.Add(part);
			}
			return new StrokeTemplate(parts);
		}

		public override string ToString()
		{
			return StrokePart.StrokeToString(Parts);
		}

		/*public static StrokeTemplate FromTreeDoc(TreeDoc doc)
		{
			List<StrokePartTemplate> parts = new List<StrokePartTemplate>();
			Vector2f? previous = null;
			float curve = 0;
			foreach (TreeDoc elem in doc.Children)
			{
				if (elem.Name == "")
				{
					float x = (float)elem.Element("", 0);
					float y = (float)elem.Element("", 1);
					Vector2f current = new Vector2f(x, y);

					if (previous != null)
					{
						parts.Add(new StrokePartTemplate(previous.Value, current, curve));
					}

					curve = 0;
					previous = current;
				}
				if (elem.Name == "Curve")
					curve = (float)elem;

			}
			return new StrokeTemplate(parts);
		}*/

		private StrokeTemplate(IEnumerable<StrokePartTemplate> parts)
		{
			Parts = new ReadOnlyCollection<StrokePartTemplate>(parts.ToArray());
		}

		public StrokeTemplate(IEnumerable<Vector2f> points, IEnumerable<float> curves)
		{
			List<StrokePartTemplate> list = new List<StrokePartTemplate>();
			var pointsArr = points.ToArray();
			var curvesArr = (curves == null)
				 ? new float[pointsArr.Length - 1]
				: curves.ToArray();

			if (pointsArr.Length != curvesArr.Length + 1)
				throw new ArgumentException("Number of points doesn't fit number of curves");

			for (int i = 0; i < pointsArr.Length - 1; i++)
			{
				list.Add(new StrokePartTemplate(pointsArr[i], pointsArr[i + 1], curvesArr[i]));
			}
			Parts = new ReadOnlyCollection<StrokePartTemplate>(list);
		}
	}
}
