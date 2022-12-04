using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Demo
{
    public class Enemy
    {
        public int hp { get { return healthManager == null ? 0 : healthManager.hp; } }
        public int maxHp { get; }
        public string name { get { return collider2D == null ? "" : collider2D.name; } }
        public bool isActive { get { return collider2D == null ? false : collider2D.isActiveAndEnabled; } }
        public List<int[]> position { get { return PointHelper.GetPosition(collider2D); } }

        [JsonIgnoreAttribute]
        public GameObject gameObject;
        private Collider2D collider2D;

        private HealthManager healthManager;

        public Enemy(GameObject gameObject, Collider2D collider2D)
        {
            this.gameObject = gameObject;
            this.collider2D = collider2D;
            healthManager = gameObject.GetComponent<HealthManager>();
            maxHp = healthManager.hp;
        }
    }
}

