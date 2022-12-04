using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class Attack
    {
        public string name { get { return collider2D == null ? "" : collider2D.name; } }
        public bool isActive { get { return collider2D == null ? false : collider2D.isActiveAndEnabled; } }
        public List<int[]> position { get { return PointHelper.GetPosition(collider2D); } }

        [JsonIgnoreAttribute]
        public GameObject gameObject;
        private Collider2D collider2D;

        public Attack(GameObject gameObject, Collider2D collider2D)
        {
            this.gameObject = gameObject;
            this.collider2D = collider2D;
        }
    }
}

