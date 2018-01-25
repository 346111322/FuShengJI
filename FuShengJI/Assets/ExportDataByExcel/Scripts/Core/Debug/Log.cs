using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

namespace BH.Core.Debug
{
    public class Log
    {
        static bool _isDebug = false;
        static bool isDebug
        {
            get
            {
#if Debug
                return true;
#else
                return _isDebug;
#endif
            }
            set { _isDebug = value; }
        }
        public static void Message(object o)
        {
            if (isDebug)
                UnityEngine.Debug.Log(o);
        }

        public static void Message(string message)
        {
            if (isDebug)
            {
                UnityEngine.Debug.Log(message);
            }
        }

        public static void Warning(string message)
        {
            if (isDebug)
            {
                UnityEngine.Debug.LogWarning(message);
            }
        }

        public static void Error(string message)
        {
            if (isDebug)
            {
                UnityEngine.Debug.LogError(message);
            }
        }
    }
}