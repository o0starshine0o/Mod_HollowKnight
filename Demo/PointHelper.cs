using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class PointHelper
    {
        public static List<int[]> GetPosition(Collider2D collider2D)
        {
            if (collider2D == null)
            {
                return new List<int[]>();
            }
            switch (collider2D)
            {
                case BoxCollider2D boxCollider2D:
                    return GetPoints(boxCollider2D);
                case PolygonCollider2D polygonCollider2D:
                    return GetPoints(polygonCollider2D);
                default:
                    return new List<int[]>();
            }
        }

        public static List<int[]> GetPoints(BoxCollider2D boxCollider2D)
        {
            Vector2 halfSize = boxCollider2D.size / 2f;
            Vector2 topLeftPoint = new(-halfSize.x, halfSize.y);
            Vector2 bottomRightPoint = new(halfSize.x, -halfSize.y);
            int[] worldTopLeft = ToWorldPoint(boxCollider2D, topLeftPoint);
            int[] worldBottomRight = ToWorldPoint(boxCollider2D, bottomRightPoint);
            return new List<int[]> { worldTopLeft, worldBottomRight };
        }

        public static List<int[]> GetPoints(PolygonCollider2D polygonCollider2D)
        {
            List<int[]> points = new List<int[]>();
            for (int i = 0; i < polygonCollider2D.pathCount; i++)
            {
                foreach (Vector2 point in polygonCollider2D.GetPath(i))
                {
                    points.Add(ToWorldPoint(polygonCollider2D, point));
                }
            }
            return points;
        }

        public static int[] ToWorldPoint(Collider2D collider2D, Vector2 point)
        {
            Vector2 offsetPoint = point + collider2D.offset;
            Vector2 transformPoint = collider2D.transform.TransformPoint(offsetPoint);
            Vector2 worldPoint = Camera.main.WorldToScreenPoint(transformPoint);
            return new int[] { (int)Math.Round(worldPoint.x), (int)Math.Round(worldPoint.y) };
        }
    }
}

