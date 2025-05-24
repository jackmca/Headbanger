using UnityEngine;

namespace Core.Singleton
{
    public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] protected bool dontDestroyOnLoad = false;

        public static T Instance { get; private set; }

        [Header("Runtime")]
        public static bool isClosing = false;

        protected virtual void Awake()
        {
            if (Instance == null) Instance = (T)FindFirstObjectByType(typeof(T));
            else
            {
                if (Instance != this)
                {
                    Destroy(Instance);
                    Instance = (T)FindFirstObjectByType(typeof(T));
                }
            }

            if (dontDestroyOnLoad)
                DontDestroyOnLoad(this);
        }

        public virtual void OnApplicationQuit()
        {
            isClosing = true;
        }

        public virtual void OnDestroy()
        {
            isClosing = true;
        }
    }
}