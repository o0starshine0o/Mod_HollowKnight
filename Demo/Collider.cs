using System;
using System.Collections.Generic;
using UnityEngine;
namespace Demo
{
    public class Collider
    {
        public Collider()
        {
        }

        // 使用单例模式
        private static Collider _instance;

        public static Collider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new();
                }
                return _instance;
            }
        }

        // 以下内容每一帧更新都需要输出
        public Knight Knight { get; set; }
        public List<Enemy> Enemies { get { return enemies.FindAll(enemy => enemy.isActive); } }
        public List<Attack> Attacks { get { return attacks.FindAll(attack => attack.isActive); } }

        private readonly List<Enemy> enemies = new List<Enemy>();
        private readonly List<Attack> attacks = new List<Attack>();

        // 创建GameObject的hook
        public void ColliderCreateHook(GameObject gameObject)
        {
            foreach (Collider2D collider2D in gameObject.GetComponentsInChildren<Collider2D>(true))
            {
                TryAddCollider(gameObject, collider2D);
            }
        }

        public void BeforeSceneLoadHook()
        {
            enemies.Clear();

            DqnMod.instance.Log("Clear all enemies and attacks");
        }

        private void TryAddCollider(GameObject gameObject, Collider2D collider2D)
        {
            if (collider2D == null)
            {
                return;
            }
            if (gameObject == HeroController.instance.gameObject && !collider2D.isTrigger)
            {
                TryAddKnight(gameObject, collider2D);
            } else if (collider2D.GetComponent<DamageHero>() || gameObject.LocateMyFSM("damages_hero"))
            {
                TryAddEnemy(gameObject, collider2D);
            } else if (gameObject.GetComponent<DamageEnemies>() || gameObject.LocateMyFSM("damages_enemy") || gameObject.name == "Damager" && gameObject.LocateMyFSM("Damage"))
            {
                TryAddAttack(gameObject, collider2D);
            }
        }

        private void TryAddKnight(GameObject gameObject, Collider2D collider2D)
        {
            Knight = new(gameObject, collider2D);

            DqnMod.instance.Log("Add knight");
        }

        private void TryAddEnemy(GameObject gameObject, Collider2D collider2D)
        {
            enemies.Add(new(gameObject, collider2D));

            DqnMod.instance.Log($"Add enemy: {gameObject.name} {collider2D.name}");
        }

        private void TryAddAttack(GameObject gameObject, Collider2D collider2D)
        {
            // 冲突区域判定, 像是判断拼刀的, 与攻击所在区域基本相同, 可以忽略掉
            if (collider2D.name == "Clash Tink")
            {
                return;
            }

            attacks.Add(new(gameObject, collider2D));

            DqnMod.instance.Log($"Add attack: {gameObject.name} {collider2D.name}");
        }
    }
}

