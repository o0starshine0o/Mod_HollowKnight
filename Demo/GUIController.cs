using System;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace Demo {
    // 需要添加到引擎中去,才能达到每帧调用的目的
    public class GUIController : MonoBehaviour {
        private static GUIController _instance;

        public static GUIController Instance {
            get {
                if (_instance == null) {
                    _instance = UnityEngine.Object.FindObjectOfType<GUIController> ();
                    if (_instance == null) {
                        DqnMod.instance.LogWarn ("[DEBUG MOD] Couldn't find GUIController");

                        GameObject GUIObj = new GameObject ();
                        _instance = GUIObj.AddComponent<GUIController> ();
                        DontDestroyOnLoad (GUIObj);
                    }
                }
                return _instance;
            }
        }

        // 这个方法每帧都要调用,成本很高
        public void Update ()
        {
            HitBox.Instance.outputKnight ();
        }
    }
}

