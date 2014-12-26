using System;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;


namespace Kangou.Core
{
	public class Point
		: MvxNotifyPropertyChanged
	{
		public Point(double x, double y)
		{
			X = x;
			Y = y;
		}

		public double X { get; set; }
		public double Y { get; set; }
	}

	public class GpsArea
	{
		private static Dictionary<string, Point[]> _areas = new Dictionary<string, Point[]>
		{
			{
				"México D.F.", new Point[] 
				{ 
					new Point(19.528002, -99.285882),
					new Point(19.553239, -99.018090),
					new Point(19.301990, -98.978952),
					new Point(19.266668, -99.291719)
				}
			}
		};

		private static bool IsInPolygon(Point[] poly, Point p)
		{
			Point p1, p2;

			bool inside = false;


			if (poly.Length < 3)
			{
				return inside;
			}


			var oldPoint = new Point(
				poly[poly.Length - 1].X, poly[poly.Length - 1].Y);


			for (int i = 0; i < poly.Length; i++)
			{
				var newPoint = new Point(poly[i].X, poly[i].Y);


				if (newPoint.X > oldPoint.X)
				{
					p1 = oldPoint;

					p2 = newPoint;
				}

				else
				{
					p1 = newPoint;

					p2 = oldPoint;
				}


				if ((newPoint.X < p.X) == (p.X <= oldPoint.X)
					&& (p.Y - (double) p1.Y)*(p2.X - p1.X)
					< (p2.Y - (double) p1.Y)*(p.X - p1.X))
				{
					inside = !inside;
				}


				oldPoint = newPoint;
			}

			return inside;
		}

		public static bool IsWithinSomeAvailableArea(double lat, double lng)
		{
			var location = new Point (lat,lng);

			foreach (KeyValuePair<string, Point[]> entry in _areas) {
				Debug.WriteLine ("Area: {0}",entry.Key);
				if (!IsInPolygon (entry.Value, location))
					return false;
			}

			return true;
		}
	}
}

