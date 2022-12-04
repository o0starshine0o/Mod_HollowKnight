using System;
using UnityEngine;
using System.Collections.Generic;

namespace Demo
{
    public class Knight
    {
        public List<int[]> position { get { return PointHelper.GetPosition(collider2D); } }
        public int hp { get { return HeroController.instance.playerData.health; } }
        public bool canCast { get { return HeroController.instance.CanCast(); } }

        private GameObject gameObject;
        private Collider2D collider2D;

        public Knight(GameObject gameObject, Collider2D collider2D)
        {
            this.gameObject = gameObject;
            this.collider2D = collider2D;
        }
    }
}

