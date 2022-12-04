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

        public Collider collider { get { return Collider.Instance; } }
    }
}

