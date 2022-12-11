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

namespace Demo
{
    public class DqnMod : Mod
    {
        // 使用单例, 方便其他位置调取
        public static DqnMod instance;

        private static byte[] _buffer = new byte[2048];

        private const int _modVersion = 55;

        private Socket _socket;

        public DqnMod()
        {
            instance = this;
        }

        public override string GetVersion() => $"{ModHooks.ModVersion}:{_modVersion}";

        public override void Initialize()
        {
            Log("Hello World");

            Socket();

            ModHooks.BeforeSceneLoadHook += BeforeSceneLoadHook;

            ModHooks.ColliderCreateHook += ColliderCreateHook;
        }

        public void Send(string message)
        {
            Log(message);
            _socket.Send(System.Text.Encoding.Default.GetBytes(message));
            _socket.Receive(_buffer);
        }

        // 初始化socket, 方便其他模块直接向socket输出内容
        private void Socket()
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9203);

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(ipEndPoint);

            // 测试连接是否完成
            Send("Make AI Great Again too!");
        }

        private string BeforeSceneLoadHook(string scene)
        {
            // 进入场景回调，可以考虑脚本控制角色或者开始AI
            // GG, GODS_GLORY, 神居
            // GG_Atrium， 中庭
            // GG_Workshop, 作坊
            // GG_Hornet_2， 大黄蜂
            Log($"BeforeSceneLoadHook: {scene}");

            Collider.Instance.BeforeSceneLoadHook();
            Message.Instance.scene = scene;
            GUIController.Instance.Update();

            return scene;
        }

        private void ColliderCreateHook(GameObject gameObject)
        {
            // 创建碰撞体回调, 包含了knight, ememy等
            Collider.Instance.ColliderCreateHook(gameObject);
            Log($"Add Collider: {gameObject.name}");
        }
    }
}

