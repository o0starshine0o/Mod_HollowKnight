using System;
using System.Collections.Generic;

namespace Demo
{
    public class Message
    {
        private static Message _instance;

        public static Message Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Message();
                }
                return _instance;
            }
        }

        public string scene { get; set; }

        public string time
        {
            get
            {
                return $"{DateTime.Now}.{DateTime.Now.Millisecond.ToString("000")}";
            }
        }

        public float[] knight_points
        {
            get
            {
                return HitBox.Instance.GetKnightPoints();
            }
        }

        public List<float[]> enemy_points
        {
            get
            {
                return HitBox.Instance.GetEnemyPoints();
            }
        }

        public int hp
        {
            get
            {
                return PlayerData.instance.health;
            }
        }

        public List<Enemy> enemies
        {
            get
            {
                return EnemyData.Instance.Update();
            }
        }
    }
}

