using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Logging
{
    public static class GameLog
    {
        public static void LogMessage(string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }

        public static void LogMessage(string message, Object context)
        {
#if UNITY_EDITOR
            Debug.Log(message, context);
#endif
        }

        public static void LogWarning(string message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif
        }

        public static void LogError(string message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif
        }

        public static void LogException(Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogException(ex);
#endif
        }
    }
}