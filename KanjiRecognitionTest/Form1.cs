using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chaos.Util.Mathematics;
using Chaos.Util.TreeDocuments;

namespace KanjiRecognitionTest
{
	public partial class Form1 : Form
	{

		public Form1()
		{
			InitializeComponent();
			clear_Click(null, null);
			TreeDoc td = TreeDoc.Load("KanjiDb.txt");
			foreach (var kanjiTd in td.Children)
			{
				//if ( kanjiTd.Name != "Mouth")
				//	continue;
				var template = new List<StrokeTemplate>();
				foreach (var strokeTd in kanjiTd.Children)
				{
					template.Add(StrokeTemplate.FromTreeDoc(strokeTd));
				}
				templates.Add(template);
			}
			matchedTemplate = templates.First();
		}

		List<Vector2f> currentStroke;
		List<List<Vector2f>> allStrokes;
		List<List<StrokeTemplate>> templates = new List<List<StrokeTemplate>>();
		List<StrokeTemplate> matchedTemplate;
		List<StrokeMatch> bestMatch;

		private void clear_Click(object sender, EventArgs e)
		{
			allStrokes = new List<List<Vector2f>>();
			currentStroke = null;
			kanjiImage.Invalidate();
		}

		private void kanjiImage_MouseMove(object sender, MouseEventArgs e)
		{
			bool down = (MouseButtons & MouseButtons.Left) != 0;
			if (down)
			{
				if (currentStroke == null)
				{
					currentStroke = new List<Vector2f>();
					allStrokes.Add(currentStroke);
				}
				currentStroke.Add(new Vector2f(e.X, e.Y));
				match_Click(null, null);
				kanjiImage.Invalidate();
			}
			else
			{
				currentStroke = null;
			}
		}

		private void DrawStrokePart(StrokePart part, Vector2f topLeft, float scale, Graphics graphics, Pen pen1, Pen pen2)
		{
			graphics.DrawLine(pen1, new PointF(part.StartPoint.X, part.StartPoint.Y), new PointF(part.EndPoint.X, part.EndPoint.Y));
			const int sections = 64;
			PointF[] roundPoints = new PointF[sections + 1];
			Vector2f delta = part.EndPoint - part.StartPoint;
			Vector2f ortho = new Vector2f(delta.Y, -delta.X);
			for (int i = 0; i <= sections; i++)
			{
				float pos = (float)i / sections;
				float param = 1 - (2 * pos - 1) * (2 * pos - 1);
				Vector2f point = part.StartPoint + delta * pos + ortho * param * (float)part.Curve / 2;
				point = topLeft + point * scale;
				roundPoints[i] = new PointF(point.X, point.Y);
			}
			graphics.DrawLines(pen2, roundPoints);
		}

		private void kanjiImage_Paint(object sender, PaintEventArgs e)
		{
			try
			{
				foreach (var stroke in matchedTemplate)
				{
					foreach (var part in stroke.Parts)
					{
						DrawStrokePart(part, new Vector2f(30, 20), 100, e.Graphics, Pens.Transparent, Pens.Green);
					}
				}
				if (bestMatch != null)
					foreach (StrokeMatch match in bestMatch)
					{
						foreach (StrokePartMatch part in match.Parts)
						{
							DrawStrokePart(part, new Vector2f(30, 20), 100, e.Graphics, Pens.Red, Pens.Blue);
						}
					}
				/*foreach (var inputStroke in allStrokes)
				{
					var match = StrokeMatcher.Match(inputStroke).ArgMaxOrDefault(m => -m.Cost);
					if (match != null)
					{
						foreach (var part in match.Parts)
						{
							DrawStrokePart(part, new Vector2f(0, 0), 1, e.Graphics, Pens.Red, Pens.Blue);
						}
					}
				}*/
				foreach (var stroke in allStrokes)
				{
					if (stroke == null)
						continue;
					PointF[] points = stroke.Select(v => new PointF(v.X, v.Y)).ToArray();
					if (points.Length >= 2)
					{
						e.Graphics.DrawLines(Pens.Black, points);
					}
				}
			}
			catch (Exception ex)//hack
			{

			}
		}

		private void match_Click(object sender, EventArgs e)
		{
			float bestMatchCost = float.PositiveInfinity;
			foreach (var template in templates)
			{
				List<StrokeMatch> match;
				float cost = KanjiMatcher.Match(allStrokes, template, out match);
				if (cost < bestMatchCost)
				{
					bestMatchCost = cost;
					bestMatch = match;
					matchedTemplate = template;
				}
			}
			matchQuality.Text = bestMatchCost.ToString();
		}

	}
}
