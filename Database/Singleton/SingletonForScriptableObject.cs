using DG.Tweening.Plugins.Core.PathCore;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Database.Singleton
{
    public class SingletonForScriptableObject<T> : SerializedScriptableObject where T : SerializedScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] private string _description = typeof(T).Name;
#endif

        private static T _instance;
        private static string _pathName;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<T>(typeof(T).Name);

                    if (_instance == null)
                        Debug.LogError($"Resource \"{typeof(T).Name}\" not found.");
                }

                return _instance;
            }
        }
    }
}