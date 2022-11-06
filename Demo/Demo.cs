using System;
using System.Reflection;
using GlobalEnums;
using Modding;

namespace Demo {
    public class ClassTest : Mod {

        private const int _modVersion = 3;

        public override string GetVersion () => $"{ModHooks.ModVersion}:{_modVersion}";

        public override void Initialize ()
        {
            Log ("Hello World");

            ModHooks.SoulGainHook += SoulGainHook;

            ModHooks.AfterAttackHook += AfterAttackHook;

            ModHooks.AfterPlayerDeadHook += AfterPlayerDeadHook;

            ModHooks.AfterTakeDamageHook += AfterTakeDamageHook;
        }

        private int SoulGainHook (int soul)
        {
            // 魂值获取， 攻击到11， 被攻击到6
            Log ($"SoulGainHook: {soul}");
            return soul;
        }

        private void AfterAttackHook (AttackDirection attackDirection)
        {
            // 没啥用， 显示攻击的方向
            Log ($"AfterAttackHook: {attackDirection}");
        }

        private void AfterPlayerDeadHook ()
        {
            // 没啥用， 没有入参，没有返回值
            Log ($"AfterPlayerDeadHook");
        }

        private int AfterTakeDamageHook (int hazardType, int damageAmount)
        {
            // 没啥用， 没有入参，没有返回值
            Log ($"AfterTakeDamageHook{hazardType}: {damageAmount}");
            return damageAmount;
        }
    }
}

