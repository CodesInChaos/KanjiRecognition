using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Chaos.Util.Mathematics;
using Chaos.Util.TreeDocuments;

namespace KanjiRecognitionTest
{
	class StrokeTemplate
	{
		public readonly ReadOnlyCollection<StrokePartTemplate> Parts;

		public static StrokeTemplate FromTreeDoc(TreeDoc doc)
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
		}

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
