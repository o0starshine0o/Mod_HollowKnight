using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using GlobalEnums;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo {
    public class DqnMod : Mod {
        // 使用单例, 方便其他位置调取
        public static DqnMod instance;

        public Socket socket;

        private const int _modVersion = 20;

        public DqnMod ()
        {
            instance = this;
        }

        public override string GetVersion () => $"{ModHooks.ModVersion}:{_modVersion}";

        public override void Initialize ()
        {
            Log ("Hello World");

            Socket ();

            GUIController.Instance.Update ();

            ModHooks.SoulGainHook += SoulGainHook;

            ModHooks.AfterAttackHook += AfterAttackHook;

            ModHooks.AfterPlayerDeadHook += AfterPlayerDeadHook;

            ModHooks.AfterTakeDamageHook += AfterTakeDamageHook;

            ModHooks.AttackHook += AttackHook;

            ModHooks.BeforeSceneLoadHook += BeforeSceneLoadHook;

            ModHooks.CharmUpdateHook += CharmUpdateHook;

            ModHooks.ColliderCreateHook += ColliderCreateHook;

            ModHooks.DrawBlackBordersHook += DrawBlackBordersHook;
        }

        // 初始化socket, 方便其他模块直接向socket输出内容
        private void Socket ()
        {
            var ipEndPoint = new IPEndPoint (IPAddress.Parse ("127.0.0.1"), 9203);

            socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect (ipEndPoint);


            //socket.Send (System.Text.Encoding.Default.GetBytes ("Make AI Great Again too!"));
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
            // 被BOSS攻击到的回调， type-1， damage-2
            Log ($"AfterTakeDamageHook({hazardType}): {damageAmount}");
            return damageAmount;
        }

        private void AttackHook (AttackDirection attackDirection)
        {
            Log ($"AttackHook: {attackDirection}");
        }

        private string BeforeSceneLoadHook (string scene)
        {
            // 进入场景回调，可以考虑脚本控制角色或者开始AI
            // GG, GODS_GLORY, 神居
            // GG_Atrium， 中庭
            // GG_Workshop, 作坊
            // GG_Hornet_2， 大黄蜂
            Log ($"BeforeSceneLoadHook: {scene}");
            //socket.Send (System.Text.Encoding.Default.GetBytes(scene));

            socket.Send (System.Text.Encoding.Default.GetBytes ("Make AI Great Again too!"));
            return scene;
        }

        private void CharmUpdateHook (PlayerData data, HeroController controller)
        {
            // 护符更新时回调
            // PlayerData数据量很多, HeroController数据量也很多
            Log ($"CharmUpdateHook: {data}, {controller}");
        }

        private void ColliderCreateHook (GameObject gameObject) =>
            HitBox.Instance.UpdateHitBox (gameObject);


        private void DrawBlackBordersHook (List<GameObject> gameObjects)
        {
            // 目前还没看到哪里有使用, 先放着吧, 以后再看
            foreach (GameObject game in gameObjects) {
                Log ($"DrawBlackBordersHook: {game.name}");
            }
        }
    }
}

