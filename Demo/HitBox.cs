using System;
using System.Collections.Generic;
using GlobalEnums;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace Demo {
    public class HitBox {
        // 内部类, 碰撞检测的类型
        private struct HitboxType : IComparable<HitboxType> {
            public static readonly HitboxType Knight = new (Color.yellow, 0);                     // yellow
            public static readonly HitboxType Enemy = new (new Color (0.8f, 0, 0), 1);       // red      
            public static readonly HitboxType Attack = new (Color.cyan, 2);                       // cyan
            public static readonly HitboxType Terrain = new (new Color (0, 0.8f, 0), 3);     // green
            public static readonly HitboxType Trigger = new (new Color (0.5f, 0.5f, 1f), 4); // blue
            public static readonly HitboxType Breakable = new (new Color (1f, 0.75f, 0.8f), 5); // pink
            public static readonly HitboxType Gate = new (new Color (0.0f, 0.0f, 0.5f), 6); // dark blue
            public static readonly HitboxType HazardRespawn = new (new Color (0.5f, 0.0f, 0.5f), 7); // purple 
            public static readonly HitboxType Other = new (new Color (0.9f, 0.6f, 0.4f), 8); // orange


            public readonly Color Color;
            public readonly int Depth;

            private HitboxType (Color color, int depth)
            {
                Color = color;
                Depth = depth;
            }

            public int CompareTo (HitboxType other)
            {
                return other.Depth.CompareTo (Depth);
            }
        }

        // 使用单例模式

        private static HitBox _instance;

        public static HitBox Instance {
            get {
                if (_instance == null) {
                    _instance = new ();
                }
                return _instance;
            }
        }


        // 构建一个HashMap, 保存所有类型的Collider2D
        private readonly SortedDictionary<HitboxType, HashSet<Collider2D>> colliders = new ()
        {
            {HitboxType.Knight, new HashSet<Collider2D>()},
            {HitboxType.Enemy, new HashSet<Collider2D>()},
            {HitboxType.Attack, new HashSet<Collider2D>()},
            {HitboxType.Terrain, new HashSet<Collider2D>()},
            {HitboxType.Trigger, new HashSet<Collider2D>()},
            {HitboxType.Breakable, new HashSet<Collider2D>()},
            {HitboxType.Gate, new HashSet<Collider2D>()},
            {HitboxType.HazardRespawn, new HashSet<Collider2D>()},
            {HitboxType.Other, new HashSet<Collider2D>()},
        };


        // 当有新的Collider2D进入时, 保存下来
        public void UpdateHitBox (GameObject go)
        {
            foreach (Collider2D collider2D in go.GetComponentsInChildren<Collider2D> (true)) {
                TryAddHitboxes (collider2D);
            }
        }

        public void ClearHitBox ()
        {
            foreach (HashSet<Collider2D> hashSet in colliders.Values) {
                hashSet.Clear ();
            }
            DqnMod.instance.Log ($"ClearHitBox");
        }

        // 把collider2D装入不同的set
        private void TryAddHitboxes (Collider2D collider2D)
        {
            if (collider2D == null) {
                return;
            }

            if (collider2D is BoxCollider2D or PolygonCollider2D or EdgeCollider2D or CircleCollider2D) {
                GameObject go = collider2D.gameObject;
                if (collider2D.GetComponent<DamageHero> () || collider2D.gameObject.LocateMyFSM ("damages_hero")) {
                    AddHitBox (HitboxType.Enemy, collider2D);
                } else if (go.GetComponent<HealthManager> () || go.LocateMyFSM ("health_manager_enemy") || go.LocateMyFSM ("health_manager")) {
                    AddHitBox (HitboxType.Other, collider2D);
                } else if (go.layer == (int)PhysLayers.TERRAIN) {
                    if (go.name.Contains ("Breakable") || go.name.Contains ("Collapse") || go.GetComponent<Breakable> () != null) {
                        AddHitBox (HitboxType.Breakable, collider2D);
                    } else {
                        AddHitBox (HitboxType.Terrain, collider2D);
                    }
                } else if (go == HeroController.instance?.gameObject && !collider2D.isTrigger) {
                    AddHitBox (HitboxType.Knight, collider2D);
                } else if (go.GetComponent<DamageEnemies> () || go.LocateMyFSM ("damages_enemy") || go.name == "Damager" && go.LocateMyFSM ("Damage")) {
                    AddHitBox (HitboxType.Attack, collider2D);
                } else if (collider2D.isTrigger && collider2D.GetComponent<HazardRespawnTrigger> ()) {
                    AddHitBox (HitboxType.HazardRespawn, collider2D);
                } else if (collider2D.isTrigger && collider2D.GetComponent<TransitionPoint> ()) {
                    AddHitBox (HitboxType.Gate, collider2D);
                } else if (collider2D.GetComponent<Breakable> ()) {
                    NonBouncer bounce = collider2D.GetComponent<NonBouncer> ();
                    if (bounce == null || !bounce.active) {
                        AddHitBox (HitboxType.Trigger, collider2D);
                    }
                } else {
                    AddHitBox (HitboxType.Other, collider2D);
                }
            }
        }

        private void AddHitBox (HitboxType hitboxType, Collider2D collider2D)
        {
            colliders[hitboxType].Add (collider2D);
            // DqnMod.instance.Log ($"TryAddHitboxes[{hitboxType}]: {collider2D}");
        }

        // 获取knight的坐标信息
        public float [] GetKnightPoints ()
        {
            foreach (Collider2D collider2D in colliders [HitboxType.Knight]) {
                switch (collider2D) {
                case BoxCollider2D boxCollider2D:
                    return GetPoints (boxCollider2D);
                }
            }
            return null;
        }

        // 获取敌人的坐标信息
        public List<float []> GetEnemyPoints ()
        {
            List<float []> points = new List<float []> ();
            foreach (Collider2D collider2D in colliders [HitboxType.Enemy]) {
                switch (collider2D) {
                case BoxCollider2D boxCollider2D:
                    points.Add (GetPoints (boxCollider2D));
                    break;
                case PolygonCollider2D polygonCollider2D:
                    points.AddRange (GetPoints (polygonCollider2D));
                    break;
                }
            }
            return points;
        }

        // 获取knight的坐标信息
        public string GetKnightDesc ()
        {
            foreach (Collider2D collider2D in colliders [HitboxType.Knight]) {
                switch (collider2D) {
                case BoxCollider2D boxCollider2D:
                    return GetPointDesc (boxCollider2D);
                }
            }
            return "";
        }

        // 获取敌人的坐标信息
        public string GetEnemyDesc ()
        {
            string desc = "";
            foreach (Collider2D collider2D in colliders [HitboxType.Enemy]) {
                switch (collider2D) {
                case BoxCollider2D boxCollider2D:
                    desc += GetPointDesc (boxCollider2D);
                    break;
                case PolygonCollider2D polygonCollider2D:
                    desc += GetPointDesc (polygonCollider2D);
                    break;
                }
            }

            return desc;
        }

        private float[] GetPoints(BoxCollider2D boxCollider2D)
        {
            Vector2 halfSize = boxCollider2D.size / 2f;
            Vector2 topLeftPoint = new (-halfSize.x, halfSize.y);
            Vector2 bottomRightPoint = new (halfSize.x, -halfSize.y);
            Vector2 topLeft = ToWorldPoint (boxCollider2D, topLeftPoint);
            Vector2 bottomRight = ToWorldPoint (boxCollider2D, bottomRightPoint);
            return new float [4] {topLeft.x,topLeft.y,bottomRight.x, bottomRight.y};
        }

        private List<float []> GetPoints (PolygonCollider2D polygonCollider2D)
        {
            List<float []> points = new List<float[]>();
            for (int i = 0; i < polygonCollider2D.pathCount; i++) {
                Vector2 topLeftPoint = new (float.MaxValue, float.MinValue);
                Vector2 bottomRightPoint = new (float.MinValue, float.MaxValue);
                Vector2 [] polygonPoints = polygonCollider2D.GetPath (i);
                // 选取最边界的情况, 使用矩形代替多边形
                foreach (Vector2 point in polygonPoints) {
                    if (point.x < topLeftPoint.x) topLeftPoint.x = point.x;
                    if (point.y > topLeftPoint.y) topLeftPoint.y = point.y;
                    if (point.x > bottomRightPoint.x) bottomRightPoint.x = point.x;
                    if (point.y < bottomRightPoint.y) bottomRightPoint.y = point.y;
                }
                Vector2 topLeft = ToWorldPoint (polygonCollider2D, topLeftPoint);
                Vector2 bottomRight = ToWorldPoint (polygonCollider2D, bottomRightPoint);
                points.Add (new float [4] { topLeft.x, topLeft.y, bottomRight.x, bottomRight.y });
            }
            return points;
        }

        private string GetPointDesc (BoxCollider2D boxCollider2D)
        {
            Vector2 halfSize = boxCollider2D.size / 2f;
            Vector2 topLeftPoint = new (-halfSize.x, halfSize.y);
            Vector2 bottomRightPoint = new (halfSize.x, -halfSize.y);
            Vector2 topLeft = ToWorldPoint (boxCollider2D, topLeftPoint);
            Vector2 bottomRight = ToWorldPoint (boxCollider2D, bottomRightPoint);
            return $"{topLeft.x},{topLeft.y},{bottomRight.x},{bottomRight.y},";
        }

        private string GetPointDesc (PolygonCollider2D polygonCollider2D)
        {
            string desc = "";
            for (int i = 0; i < polygonCollider2D.pathCount; i++) {
                Vector2 topLeftPoint = new (float.MaxValue, float.MinValue);
                Vector2 bottomRightPoint = new (float.MinValue, float.MaxValue);
                Vector2 [] polygonPoints = polygonCollider2D.GetPath (i);
                // 选取最边界的情况, 使用矩形代替多边形
                foreach(Vector2 point in polygonPoints) {
                    if (point.x < topLeftPoint.x) topLeftPoint.x = point.x;
                    if (point.y > topLeftPoint.y) topLeftPoint.y = point.y;
                    if (point.x > bottomRightPoint.x) bottomRightPoint.x = point.x;
                    if (point.y < bottomRightPoint.y) bottomRightPoint.y = point.y;
                }
                Vector2 topLeft = ToWorldPoint (polygonCollider2D, topLeftPoint);
                Vector2 bottomRight = ToWorldPoint (polygonCollider2D, bottomRightPoint);
                desc += $"{topLeft.x},{topLeft.y},{bottomRight.x},{bottomRight.y},";
            }
            return desc;
        }

        private Vector2 ToWorldPoint (Collider2D collider2D, Vector2 point)
        {
            Vector2 offsetPoint = point + collider2D.offset;
            Vector2 transformPoint = collider2D.transform.TransformPoint (offsetPoint);
            Vector2 worldPoint = Camera.main.WorldToScreenPoint (transformPoint);
            worldPoint.y -= Screen.height;
            return worldPoint;
        }
    }
}

