using System;
using HutongGames.PlayMaker.Actions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo
{
    // 需要添加到引擎中去,才能达到每帧调用的目的
    public class GUIController : MonoBehaviour
    {
        private static GUIController _instance;

        public static GUIController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = UnityEngine.Object.FindObjectOfType<GUIController>();
                    if (_instance == null)
                    {
                        DqnMod.instance.LogWarn("[DEBUG MOD] Couldn't find GUIController");

                        GameObject GUIObj = new GameObject();
                        _instance = GUIObj.AddComponent<GUIController>();
                        DontDestroyOnLoad(GUIObj);
                    }
                }
                return _instance;
            }
        }

        private long _frequency = 0;

        // 这个方法每帧都要调用,成本很高
        public void Update()
        {
            // 增加一个频控, 避免过多的输出
            _frequency += 1;
            if (_frequency % 4 != 0)
            {
                return;
            }
            string jsonString = JsonConvert.SerializeObject(Message.Instance);
            DqnMod.instance.Send(jsonString);
        }
    }
}

