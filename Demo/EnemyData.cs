using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class EnemyData
    {
        private static EnemyData _instance;

        public List<Enemy> enemies { get; } = new List<Enemy>();

        public static EnemyData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EnemyData();
                }
                return _instance;
            }
        }

        public void Add(GameObject gameObject)
        {
            if (!gameObject.name.Contains("Boss")) { return; }

            int index = enemies.FindIndex(enemy => enemy.name == gameObject.name);
            if (index == -1)
            {
                enemies.Add(new Enemy(gameObject));
            }
        }

        public List<Enemy> Update()
        {
            enemies.ForEach(enemy => enemy.Update());
            return enemies;
        }

        public void Clear()
        {
            enemies.Clear();
        }
    }

    public class Enemy
    {

        public int hp { get; set; }
        public int maxHp { get; set; }
        public string name { get; set; }
        private HealthManager healthManager;
        private GameObject gameObject;

        public Enemy(GameObject gameObject)
        {
            this.gameObject = gameObject;
            name = gameObject.name;
            healthManager = gameObject.GetComponent<HealthManager>();
            hp = healthManager.hp;
            maxHp = healthManager.hp;
        }

        public void Update()
        {
            hp = healthManager.hp;
        }
    }
}

