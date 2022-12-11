using System;
using UnityEngine;
using System.Collections.Generic;

namespace Demo
{
    public class Knight
    {
        public List<int[]> position { get { return PointHelper.GetPosition(collider2D); } }

        public int hp { get { return PlayerData.instance.health; } }
        public int mp { get { return PlayerData.instance.MPCharge + PlayerData.instance.MPReserve; } }

        // 当前状态: 地面, 空闲, 跑步, 空降, 抓墙下滑, 硬着陆, 冲刺着陆, 无输入, 上一次
        public string state { get { return HeroController.instance.hero_state.ToString(); } }
        // 加速度
        public float[] velocity { get { return new float[] { HeroController.instance.current_velocity.x, HeroController.instance.current_velocity.y }; } }

        // 无敌, 被伤害后一段时间
        public bool invulnerable { get { return HeroController.instance.cState.invulnerable; } }
        // 冲刺
        public bool dashing { get { return HeroController.instance.cState.dashing; } }
        // 黑冲
        public bool superDashing { get { return HeroController.instance.cState.superDashing; } }
        // 跳跃
        public bool jumping { get { return HeroController.instance.cState.jumping; } }
        // 二段跳跃
        public bool doubleJumping { get { return HeroController.instance.cState.doubleJumping; } }
        // 下落
        public bool falling { get { return HeroController.instance.cState.falling; } }
        // 正在攻击
        public bool attacking { get { return HeroController.instance.cState.attacking; } }
        // 是否接触到墙了
        public bool touchingWall { get { return HeroController.instance.cState.touchingWall; } }
        // 是否面朝右方
        public bool facingRight { get { return HeroController.instance.cState.facingRight; } }

        // 能否冲刺
        public bool canCast { get { return HeroController.instance.CanCast(); } }
        // 能否黑冲
        public bool canSuperDash { get { return HeroController.instance.CanSuperDash(); } }

        private GameObject gameObject;
        private Collider2D collider2D;

        public Knight(GameObject gameObject, Collider2D collider2D)
        {
            this.gameObject = gameObject;
            this.collider2D = collider2D;
        }
    }
}

