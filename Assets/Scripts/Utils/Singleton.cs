using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit." + " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    var objs = FindObjectsOfType<T>();
                    if (objs.Length > 0)
                    {
                        _instance = objs[0];

                        if (objs.Length > 1)
                        {
                            Debug.LogError("[Singleton] There is more than one " + typeof(T).Name + " in the scene.");
                        }
                    }
                    else
                    {
                        Debug.LogError("[Singleton] There is no " + typeof(T).Name + " in the scene.");
                    }
                }

                return _instance;
            }
        }
    }

    private static bool _applicationIsQuitting = false;
    public void OnDestroy()
    {
        _applicationIsQuitting = true;
    }
}

