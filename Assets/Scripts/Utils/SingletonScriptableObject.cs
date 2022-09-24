using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                T[] assets = Resources.LoadAll<T>("");
                if (assets == null || assets.Length < 1)
                {
                    throw new System.Exception("Could not find any singleton scriptable object instances in the resources");
                }
                else if (assets.Length > 1)
                {
                    throw new System.Exception("Multiple instances of singleton scriptable object found in the resources");
                }
                else
                {
                    instance = assets[0];
                }
            }
            return instance;
        }
    }
}
